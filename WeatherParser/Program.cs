using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace WeatherParser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            SetEncoding();
            var cityWeatherData = await ParseCitiesWeatherAsync();
            PrintCityWeather(cityWeatherData);
            SaveWeatherDataToJson(cityWeatherData);
        }

        public static async Task<Dictionary<string, List<WeatherEntry>>> ParseCitiesWeatherAsync()
        {
            var cityWeatherData = new Dictionary<string, List<WeatherEntry>>();

            try
            {
                var httpClient = new HttpClient();
                var site = "https://www.gismeteo.ru";
                var html = await httpClient.GetStringAsync(site);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var popularCitiesNode = doc.DocumentNode.SelectSingleNode("//div[@class='list']");
                if (popularCitiesNode != null)
                {
                    var cityLinks = popularCitiesNode.SelectNodes(".//a[@class='link']");
                    if (cityLinks != null)
                    {
                        var cityWeatherTasks = cityLinks.Select(async cityLink =>
                        {
                            var cityName = cityLink.InnerText.Trim();
                            var cityUrl = site + cityLink.GetAttributeValue("href", "");
                            var cityWeatherUrl = cityUrl + "10-days/";
                            var weatherData = await ParseCityWeatherAsync(httpClient, cityWeatherUrl);

                            if (weatherData != null && weatherData.Any())
                            {
                                cityWeatherData[cityName] = weatherData;
                            }
                            else
                            {
                                Console.WriteLine($"No weather data found for {cityName}");
                            }
                        });

                        await Task.WhenAll(cityWeatherTasks);
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve popular cities.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return cityWeatherData;
        }
        public static async Task<List<WeatherEntry>> ParseCityWeatherAsync(HttpClient httpClient, string cityWeatherUrl)
        {
            var weatherEntries = new List<WeatherEntry>();

            try
            {
                var html = await httpClient.GetStringAsync(cityWeatherUrl);
                var cityDoc = new HtmlDocument();
                cityDoc.LoadHtml(html);

                var dateContainers = cityDoc.DocumentNode.SelectNodes("//div[@class='widget-row widget-row-days-date']//div[@class='date']");

                if (dateContainers != null)
                {
                    int year = DateTime.Now.Year;
                    int formatMonth = 0;

                    foreach (var dateContainer in dateContainers)
                    {
                        var dateText = dateContainer.InnerText.Trim();
                        var dateParts = dateText.Split(' ');

                        if (dateParts.Length >= 1)
                        {
                            string day = dateParts[0];

                            if (dateParts.Length == 2)
                            {
                                string month = dateParts[1];
                                formatMonth = FormatDate(month);
                            }

                            string date = $"{day:D2} {formatMonth:D2} {year}";

                            if (DateTime.TryParseExact(date, "d MM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                            {
                                var temperatureElements = cityDoc.DocumentNode.SelectNodes("//div[@class='value style_size_m']");

                                if (temperatureElements != null)
                                {
                                    // Находим индекс текущей даты
                                    int dateIndex = dateContainers.IndexOf(dateContainer);

                                    // Используем XPath-выражение для извлечения значения максимальной температуры
                                    string maxTemperatureXPath = $"(.//div[@class='value style_size_m'])[{dateIndex + 1}]//div[@class='maxt']";
                                    string maxTemperatureCelsius = GetTemperature(cityDoc, maxTemperatureXPath);

                                    // Используем XPath-выражение для извлечения значения минимальной температуры
                                    string minTemperatureXPath = $"(.//div[@class='value style_size_m'])[{dateIndex + 1}]//div[@class='mint']";
                                    string minTemperatureCelsius = GetTemperature(cityDoc, minTemperatureXPath);

                                    if (!string.IsNullOrWhiteSpace(maxTemperatureCelsius) && !string.IsNullOrWhiteSpace(minTemperatureCelsius))
                                    {
                                        weatherEntries.Add(new WeatherEntry
                                        {
                                            Date = parsedDate.ToString("yyyy-MM-dd"),
                                            MaxTemperature = maxTemperatureCelsius,
                                            MinTemperature = minTemperatureCelsius
                                        });
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Failed to parse date: {date}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return weatherEntries;
        }

        public static string GetTemperature(HtmlDocument cityDoc, string xpath)
        {
            var temperatureCelsius = cityDoc.DocumentNode.SelectSingleNode(xpath + "/span[@class='unit unit_temperature_c']");
            if (temperatureCelsius != null)
            {
                var temperatureText = temperatureCelsius.InnerText.Trim();
                // Заменяем &minus; на -
                temperatureText = temperatureText.Replace("&minus;", "-");
                return temperatureText;
            }

            return null;
        }

        public static int FormatDate(string monthText)
        {
            // Словарь с соответствиями текстовых представлений месяцев и их числовых значений
            Dictionary<string, int> monthMappings = new Dictionary<string, int>
            {
                {"янв", 1},
                {"фев", 2},
                {"мар", 3},
                {"апр", 4},
                {"мая", 5},
                {"июн", 6},
                {"июл", 7},
                {"авг", 8},
                {"сен", 9},
                {"окт", 10},
                {"ноя", 11},
                {"дек", 12},
            };

            // Приводим входной текст к нижнему регистру, чтобы быть нечувствительными к регистру
            monthText = monthText.ToLower();

            if (monthMappings.TryGetValue(monthText, out int monthNumber))
            {
                return monthNumber;
            }

            // Если не удалось преобразовать, вернем -1 или другое значение по умолчанию
            return -1;
        }

        public static void PrintCityWeather(Dictionary<string, List<WeatherEntry>> cityWeatherData)
        {
            foreach (var cityWeatherPair in cityWeatherData)
            {
                Console.WriteLine($"City: {cityWeatherPair.Key}");
                var weatherData = cityWeatherPair.Value;
                foreach (var weatherEntry in weatherData)
                {
                    Console.WriteLine($"Date: {weatherEntry.Date}, Max Temperature: {weatherEntry.MaxTemperature}, Min Temperature: {weatherEntry.MinTemperature}");
                }
            }
        }

        public static void SetEncoding()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public static void SaveWeatherDataToJson(Dictionary<string, List<WeatherEntry>> cityWeatherData)
        {
            var json = JsonConvert.SerializeObject(cityWeatherData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("weatherdata.json", json);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace WeatherParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetEncoding();
            var cityWeatherData = ParseCitiesWeather();
            PrintCityWeather(cityWeatherData);
        }

        public static Dictionary<string, List<WeatherEntry>> ParseCitiesWeather()
        {
            var cityWeatherData = new Dictionary<string, List<WeatherEntry>>();

            try
            {
                var web = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8,
                };
                var site = "https://www.gismeteo.ru";
                var doc = web.Load(site);

                var popularCitiesNode = doc.DocumentNode.SelectSingleNode("//div[@class='list']");
                if (popularCitiesNode != null)
                {
                    var cityLinks = popularCitiesNode.SelectNodes(".//a[@class='link']");
                    if (cityLinks != null)
                    {
                        foreach (var cityLink in cityLinks)
                        {
                            var cityName = cityLink.InnerText.Trim();
                            var cityUrl = site + cityLink.GetAttributeValue("href", "");

                            // Переход на страницу с погодой за 10 дней
                            var cityWeatherUrl = cityUrl + "10-days/";
                            var weatherData = ParseCityWeather(web, cityWeatherUrl);

                            if (weatherData != null && weatherData.Any())
                            {
                                cityWeatherData[cityName] = weatherData;
                            }
                            else
                            {
                                Console.WriteLine($"No weather data found for {cityName}");
                            }
                        }
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

        public static List<WeatherEntry> ParseCityWeather(HtmlWeb web, string cityWeatherUrl)
        {
            var cityDoc = web.Load(cityWeatherUrl);
            var weatherEntries = new List<WeatherEntry>();

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
                            // Извлечь элементы с классом "value style_size_m"
                            var temperatureElements = cityDoc.DocumentNode.SelectNodes("//div[@class='value style_size_m']");

                            if (temperatureElements != null)
                            {
                                foreach (var element in temperatureElements)
                                {
                                    // Извлечь значения максимальной и минимальной температуры
                                    var maxTemperatureElement = element.SelectSingleNode(".//div[@class='maxt']");
                                    var minTemperatureElement = element.SelectSingleNode(".//div[@class='mint']");

                                    if (maxTemperatureElement != null && minTemperatureElement != null)
                                    {
                                        string maxTemperatureCelsius = GetTemperature(maxTemperatureElement);
                                        string minTemperatureCelsius = GetTemperature(minTemperatureElement);

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
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to parse date: {date}");
                        }
                    }
                }
            }

            return weatherEntries;
        }


        public static string GetTemperature(HtmlNode temperatureElement)
        {
            var temperatureCelsius = temperatureElement.SelectSingleNode(".//span[@class='unit unit_temperature_c']");
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
            foreach (var cityName in cityWeatherData.Keys)
            {
                Console.WriteLine($"City: {cityName}");
                var weatherData = cityWeatherData[cityName];
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
    }

    public class WeatherEntry
    {
        public string Date { get; set; }
        public string MaxTemperature { get; set; }
        public string MinTemperature { get; set; }
    }
}

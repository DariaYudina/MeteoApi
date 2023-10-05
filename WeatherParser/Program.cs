using System;
using System.Globalization;
using System.Text;
using HtmlAgilityPack;

namespace WeatherParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetEncoding();
            ParseCitiesWeather();
        }

        public static void ParseCitiesWeather()
        {
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

                            if (weatherData != null)
                            {
                                Console.WriteLine($"City: {cityName}");
                                foreach (var weatherEntry in weatherData)
                                {
                                    Console.WriteLine($"Date: {weatherEntry.Date}, Temperature: {weatherEntry.Temperature}");
                                }
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
        }

        public static List<WeatherEntry> ParseCityWeather(HtmlWeb web, string cityWeatherUrl)
        {
            var cityDoc = web.Load(cityWeatherUrl);
            var weatherEntries = new List<WeatherEntry>();

            // Извлечение данных о погоде за 10 дней
            //var weatherNodes = cityDoc.DocumentNode.SelectNodes("//div[contains(@class, 'widget__row_days')]");
            //if (weatherNodes != null)
            //{
            //    foreach (var weatherNode in weatherNodes)
            //    {
            //        var dateNode = weatherNode.SelectSingleNode(".//a[@class='row-item']");
            //        var temperatureNode = weatherNode.SelectSingleNode(".//div[@class='w_prec__value']");

            //        if (dateNode != null && temperatureNode != null)
            //        {
            //            var date = dateNode.InnerText.Trim();
            //            var temperature = temperatureNode.InnerText.Trim();

            //            var weatherEntry = new WeatherEntry
            //            {
            //                Date = date,
            //                Temperature = temperature
            //            };

            //            weatherEntries.Add(weatherEntry);
            //        }
            //    }
            //}

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
                            Console.WriteLine($"Parsed Date: {parsedDate}");
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

        public static void SetEncoding()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }
    }

    public class WeatherEntry
    {
        public string Date { get; set; }
        public string Temperature { get; set; }
    }
}

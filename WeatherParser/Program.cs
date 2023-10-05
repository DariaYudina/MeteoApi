using System;
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
                    OverrideEncoding = Encoding.UTF8 // Установите нужную кодировку здесь
                };
                var doc = web.Load("https://www.gismeteo.ru/");

                var popularCitiesNode = doc.DocumentNode.SelectSingleNode("//div[@class='list']");
                if (popularCitiesNode != null)
                {
                    var cityLinks = popularCitiesNode.SelectNodes(".//a[@class='link']");
                    if (cityLinks != null)
                    {
                        foreach (var cityLink in cityLinks)
                        {
                            var cityName = cityLink.InnerText.Trim();
                            var cityUrl = "https://www.gismeteo.ru" + cityLink.GetAttributeValue("href", "");
                            var temperature = ParseCityWeather(web, cityUrl);

                            if (temperature != null)
                            {
                                Console.WriteLine($"City: {cityName}, Temperature: {temperature}");
                            }
                            else
                            {
                                Console.WriteLine($"No temperature data found for {cityName}");
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

        public static string ParseCityWeather(HtmlWeb web, string cityUrl)
        {
            var cityDoc = web.Load(cityUrl);
            var temperatureNode = cityDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'unit_temperature_c')]");

            return temperatureNode?.InnerText;
        }

        public static void SetEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = enc1251;
        }
    }
}

using AutoMapper;
using Entities;
using HtmlAgilityPack;
using IMeteoLogic;
using MeteoApi.Models ;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Newtonsoft.Json;

namespace MeteoApi.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherLogic _weatherLogic;
        private readonly IMapper _mapper;
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IWeatherLogic weatherService,
            IMapper mapper)
        {
            _logger = logger;
            _weatherLogic = weatherService;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var cityWeatherData = await _weatherLogic.GetAllWeatherDataAsync();

                var weatherForecasts = cityWeatherData.Select(cityData => new
                {
                    City = cityData.City,
                    WeatherEntries = cityData.WeatherEntries
                });

                var jsonResult = JsonConvert.SerializeObject(weatherForecasts);

                return Content(jsonResult, "application/json"); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet]
        [Route("parse")]
        public IActionResult ParseWeather()
        {
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load("http://www.gismeteo.ru/");

                var weatherData = new List<string>();

                var temperatureNodes = doc.DocumentNode.SelectNodes("//span[contains(@class, 'unit_temperature_c')]");

                if (temperatureNodes != null)
                {
                    foreach (var node in temperatureNodes)
                    {
                        weatherData.Add(node.InnerText);
                    }
                }

                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка при парсинге данных о погоде.");
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> GetWeather(string city, DateTime date)
        //{
        //    try
        //    {
        //        var weatherData = await _weatherLogic.GetWeatherAsync(city, date);
        //        if (weatherData != null)
        //        {
        //            // Производите маппинг WeatherInfo в WeatherForecast
        //            var mappedWeatherData = _mapper.Map<WeatherForecast>(weatherData);
        //            return Ok(mappedWeatherData);
        //        }
        //        else
        //        {
        //            return NotFound($"Weather data not found for {city} on {date}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddWeatherData([FromBody] WeatherForecast weatherData)
        //{
        //    try
        //    {
        //        // Производите маппинг WeatherForecast в WeatherInfo
        //        var weatherInfo = _mapper.Map<WeatherInfo>(weatherData);

        //        var result = await _weatherLogic.AddWeatherDataAsync(weatherInfo);
        //        if (result.Success)
        //        {
        //            // Производите маппинг WeatherInfo в WeatherForecast для возврата
        //            var createdWeatherData = _mapper.Map<WeatherForecast>(weatherInfo);
        //            return CreatedAtAction(nameof(GetWeather), new { city = weatherInfo.City, date = weatherInfo.Date }, createdWeatherData);
        //        }
        //        else
        //        {
        //            return BadRequest(result.ErrorMessage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateWeatherData(int id, [FromBody] WeatherForecast weatherData)
        //{
        //    try
        //    {
        //        // Производите маппинг WeatherForecast в WeatherInfo
        //        var weatherInfo = _mapper.Map<WeatherInfo>(weatherData);

        //        var result = await _weatherLogic.UpdateWeatherDataAsync(id, weatherInfo);
        //        if (result.Success)
        //        {
        //            return Ok(result.Message);
        //        }
        //        else
        //        {
        //            return NotFound(result.ErrorMessage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherData(int id)
        {
            try
            {
                var result = await _weatherLogic.DeleteWeatherDataAsync(id);
                if (result.Success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}




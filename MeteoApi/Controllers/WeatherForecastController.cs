using AutoMapper;
using Entities;
using IMeteoLogic;
using MeteoApi.Models;
using Microsoft.AspNetCore.Mvc;

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
                var weatherData = await _weatherLogic.GetAllWeatherDataAsync();

                // Используйте маппер для преобразования WeatherInfo в WeatherForecast
                var weatherForecasts = _mapper.Map<IEnumerable<WeatherForecast>>(weatherData);

                // Возвращаем успешный результат с данными
                return Ok(weatherForecasts);
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если необходимо
                return StatusCode(500, $"Internal server error: {ex.Message}");
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




using Entities;
using IMeteoLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoApoLogic
{
    using AutoMapper;
    using IMeteoDao;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class WeatherLogic : IWeatherLogic
    {
        private readonly IWeatherRepository _weatherRepository; 

        public async Task<IEnumerable<CityWeatherData>> GetAllWeatherDataAsync()
        {
            try
            {
                var weatherData = await _weatherRepository.GetAllWeatherDataAsync();

                return weatherData;
            }
            catch (Exception ex)
            {
                throw new Exception("WeatherLogicException: An error occurred while getting weather data.", ex);
            }
        }

        public WeatherLogic(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public async Task<WeatherInfo> GetWeatherForecastAsync(string city, DateTime date)
        {
            // Реализуйте логику для получения прогноза погоды
            return await _weatherRepository.GetWeatherForecastAsync(city, date);
        }

        public IEnumerable<string> GetAvailableCities()
        {
            return _weatherRepository.GetAvailableCities();
        }

        public IEnumerable<DateTime> GetAvailableDates()
        {
            return _weatherRepository.GetAvailableDates();
        }

        public async Task<WeatherInfo> GetWeatherAsync(string city, DateTime date)
        {
            return await _weatherRepository.GetWeatherAsync(city, date);
        }

        //public async Task<ResultDto> AddWeatherDataAsync(WeatherInfo weatherData)
        //{
        //    // Вероятно, потребуется маппинг данных из WeatherInfoDto в WeatherInfo
        //    var result = await _weatherRepository.AddWeatherDataAsync(Mapper.MapToWeatherInfo(weatherData));

        //    return result;
        //}

        //public async Task<ResultDto> UpdateWeatherDataAsync(int id, WeatherInfo weatherData)
        //{
        //    // Вероятно, потребуется маппинг данных из WeatherInfoDto в WeatherInfo
        //    var result = await _weatherRepository.UpdateWeatherDataAsync(id, Mapper.MapToWeatherInfo(weatherData));

        //    return result;
        //}

        public async Task<ResultDto> DeleteWeatherDataAsync(int id)
        {
            var result = await _weatherRepository.DeleteWeatherDataAsync(id);

            return result;
        }


    }
}

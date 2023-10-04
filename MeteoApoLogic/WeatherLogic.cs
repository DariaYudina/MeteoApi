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
        private readonly IWeatherRepository _weatherRepository; // Предположим, что есть репозиторий для работы с данными о погоде

        public async Task<IEnumerable<WeatherInfo>> GetAllWeatherDataAsync()
        {
            try
            {
                // Здесь вы можете вызвать метод репозитория для получения всех данных о погоде
                var weatherData = await _weatherRepository.GetAllWeatherDataAsync();

                // Просто возвращаем полученные данные
                return weatherData;
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если необходимо
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
            // Реализуйте логику для получения списка доступных городов
            return _weatherRepository.GetAvailableCities();
        }

        public IEnumerable<DateTime> GetAvailableDates()
        {
            // Реализуйте логику для получения списка доступных дат
            return _weatherRepository.GetAvailableDates();
        }

        public async Task<WeatherInfo> GetWeatherAsync(string city, DateTime date)
        {
            // Реализуйте логику для получения данных о погоде
            return await _weatherRepository.GetWeatherAsync(city, date);
        }

        //public async Task<ResultDto> AddWeatherDataAsync(WeatherInfo weatherData)
        //{
        //    // Реализуйте логику для добавления данных о погоде
        //    // Вероятно, потребуется маппинг данных из WeatherInfoDto в WeatherInfo
        //    var result = await _weatherRepository.AddWeatherDataAsync(Mapper.MapToWeatherInfo(weatherData));

        //    // Верните результат операции
        //    return result;
        //}

        //public async Task<ResultDto> UpdateWeatherDataAsync(int id, WeatherInfo weatherData)
        //{
        //    // Реализуйте логику для обновления данных о погоде
        //    // Вероятно, потребуется маппинг данных из WeatherInfoDto в WeatherInfo
        //    var result = await _weatherRepository.UpdateWeatherDataAsync(id, Mapper.MapToWeatherInfo(weatherData));

        //    // Верните результат операции
        //    return result;
        //}

        public async Task<ResultDto> DeleteWeatherDataAsync(int id)
        {
            // Реализуйте логику для удаления данных о погоде
            var result = await _weatherRepository.DeleteWeatherDataAsync(id);

            // Верните результат операции
            return result;
        }


    }
}

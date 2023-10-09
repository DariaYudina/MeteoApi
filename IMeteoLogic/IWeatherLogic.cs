using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMeteoLogic
{
    public interface IWeatherLogic
    {
        Task<IEnumerable<CityWeatherData>> GetAllWeatherDataAsync();
        Task<WeatherInfo> GetWeatherForecastAsync(string city, DateTime date);

        IEnumerable<string> GetAvailableCities();

        IEnumerable<DateTime> GetAvailableDates();

        Task<WeatherInfo> GetWeatherAsync(string city, DateTime date);
        //Task<ResultDto> AddWeatherDataAsync(WeatherInfo weatherData);
        //Task<ResultDto> UpdateWeatherDataAsync(int id, WeatherInfo weatherData);
        Task<ResultDto> DeleteWeatherDataAsync(int id);
    }
}

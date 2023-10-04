using Entities;
using IMeteoDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoDAO
{
    public class WeatherRepository : IWeatherRepository
    {
        public Task<ResultDto> AddWeatherDataAsync(WeatherInfo weatherData)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto> DeleteWeatherDataAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WeatherInfo>> GetAllWeatherDataAsync()
        {
            var defaultWeatherData = new List<WeatherInfo>
            {
                new WeatherInfo
                {
                    City = "Default City 1",
                    Date = DateTime.Now.AddDays(1),
                    TemperatureC = 25,
                    Summary = "Sunny"
                },
                new WeatherInfo
                {
                    City = "Default City 2",
                    Date = DateTime.Now.AddDays(2),
                    TemperatureC = 20,
                    Summary = "Cloudy"
                },
            };

            return Task.FromResult<IEnumerable<WeatherInfo>>(defaultWeatherData);
        }

        public IEnumerable<string> GetAvailableCities()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DateTime> GetAvailableDates()
        {
            throw new NotImplementedException();
        }

        public Task<WeatherInfo> GetWeatherAsync(string city, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<WeatherInfo> GetWeatherForecastAsync(string city, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto> UpdateWeatherDataAsync(int id, WeatherInfo weatherData)
        {
            throw new NotImplementedException();
        }
    }
}

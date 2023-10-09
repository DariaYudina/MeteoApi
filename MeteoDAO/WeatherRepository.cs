using Entities;
using IMeteoDao;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoDAO
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IWeatherRepository _weatherRepository;
        private readonly MongoClient _mongoClient;
        private readonly string _databaseName;

        public WeatherRepository(MongoClient mongoClient, string databaseName)
        {
            _database = mongoClient.GetDatabase(databaseName);
            _databaseName = databaseName;
            _databaseName = _databaseName.ToLower();    
        }

        public WeatherRepository(
            IWeatherRepository weatherRepository,
            IMongoClient mongoClient, 
            string databaseName)
        {
            _weatherRepository = weatherRepository;
            _database = mongoClient.GetDatabase(databaseName);
        }

        public async Task<IEnumerable<CityWeatherData>> GetAllWeatherDataAsync()
        {
            var collection = _database.GetCollection<CityWeatherData>("WeatherInfo");
            var filter = Builders<CityWeatherData>.Filter.Empty;
            var documents = await collection.Find(filter).ToListAsync();

            var cityWeatherDataList = new List<CityWeatherData>();

            foreach (var cityData in documents)
            {
                var id = cityData.Id;
                var cityName = cityData.City;
                var weatherEntries = cityData.WeatherEntries;

                var cityWeatherData = new CityWeatherData
                {
                    Id = id,
                    City = cityName,
                    WeatherEntries = weatherEntries
                };

                cityWeatherDataList.Add(cityWeatherData);
            }

            return cityWeatherDataList;
        }

        public Task<ResultDto> AddWeatherDataAsync(WeatherInfo weatherData)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDto> DeleteWeatherDataAsync(int id)
        {
            throw new NotImplementedException();
        }

        //public Task<IEnumerable<WeatherInfo>> GetAllWeatherDataAsync()
        //{
        //    var defaultWeatherData = new List<WeatherInfo>
        //    {
        //        new WeatherInfo
        //        {
        //            City = "Default City 1",
        //            Date = DateTime.Now.AddDays(1),
        //            Summary = "Sunny"
        //        },
        //        new WeatherInfo
        //        {
        //            City = "Default City 2",
        //            Date = DateTime.Now.AddDays(2),
        //            Summary = "Cloudy"
        //        },
        //    };

        //    return Task.FromResult<IEnumerable<WeatherInfo>>(defaultWeatherData);
        //}

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

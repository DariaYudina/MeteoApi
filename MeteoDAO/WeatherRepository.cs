using Entities;
using IMeteoDao;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoDAO
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IWeatherRepository _weatherRepository;
        private MongoClient mongoClient;
        private string databaseName;

        public WeatherRepository(MongoClient mongoClient, string databaseName)
        {
            _database = mongoClient.GetDatabase(databaseName);
        }

        public WeatherRepository(
            IWeatherRepository weatherRepository,
            IMongoClient mongoClient, 
            string databaseName)
        {
            _weatherRepository = weatherRepository;
            _database = mongoClient.GetDatabase(databaseName);
        }

        public async Task<IEnumerable<WeatherInfo>> GetAllWeatherDataAsync()
        {
            var collection = _database.GetCollection<BsonDocument>("WeatherInfo");

            var filter = Builders<BsonDocument>.Filter.Empty; // Пустой фильтр для выборки всех документов
            var documents = await collection.Find(filter).ToListAsync();

            var weatherDataList = new List<WeatherInfo>();

            foreach (var document in documents)
            {
                var city = document["_id"].AsString;
                var weatherDataArray = document["weatherData"].AsBsonArray;

                foreach (var weatherData in weatherDataArray)
                {
                    var date = weatherData["date"].ToUniversalTime();
                    var temperatures = weatherData["temperatures"].AsBsonDocument;
                    var summary = weatherData["summary"].AsString;

                    // Преобразование данных и создание объекта WeatherInfo
                    var weatherInfo = new WeatherInfo
                    {
                        City = city,
                        Date = date,
                        TemperatureC = temperatures["день"].AsInt32, // Выбирайте нужное время суток
                        Summary = summary
                    };

                    weatherDataList.Add(weatherInfo);
                }
            }

            return weatherDataList;
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
        //            TemperatureC = 25,
        //            Summary = "Sunny"
        //        },
        //        new WeatherInfo
        //        {
        //            City = "Default City 2",
        //            Date = DateTime.Now.AddDays(2),
        //            TemperatureC = 20,
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

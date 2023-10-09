using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace MeteoApi.Models
{
    public class CityDataModel
    {
        [BsonId]
        [JsonIgnore] // Этот атрибут исключает поле Id из JSON-сериализации
        public ObjectId Id { get; set; }
        public string City { get; set; }
        public List<WeatherForecastModel> WeatherEntries { get; set; }
    }
}

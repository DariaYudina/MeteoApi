﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Entities
{
    public class CityWeatherData
    {
        [BsonId]
        [JsonIgnore] // Этот атрибут исключает поле Id из JSON-сериализации
        public ObjectId Id { get; set; }
        public string City { get; set; }
        public List<WeatherEntry> WeatherEntries { get; set; }
    }
}

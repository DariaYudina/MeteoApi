using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CityWeatherData
    {
        public string CityName { get; set; }
        public List<WeatherInfo> Weather { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherParser
{
    public class WeatherData
    {
        public string City { get; set; }
        public DateTime Date { get; set; }
        public string Temperature { get; set; }
        public string Summary { get; set; }
    }
}

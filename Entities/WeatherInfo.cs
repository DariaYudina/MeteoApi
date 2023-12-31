﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class WeatherInfo
    {
        public string City { get; set; }
        public DateTime Date { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public double Wind { get; set; }
        public string Summary { get; set; }
    }
}

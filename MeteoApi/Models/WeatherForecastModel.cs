namespace MeteoApi.Models
{
    public class WeatherForecastModel
    {
        public DateTime Date { get; set; }
        public double MaxTemperatureС { get; set; }
        public double MinTemperatureС { get; set; }
        public int MaxTemperatureF => 32 + (int)(MaxTemperatureС / 0.5556);
        public int MinTemperatureF => 32 + (int)(MinTemperatureС / 0.5556);
        public double Wind { get; set; }
        public string Summary { get; set; }
    }
}
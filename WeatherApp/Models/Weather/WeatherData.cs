namespace WeatherApp.Models.Weather
{
    public class WeatherData
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public double Temp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public double Speed { get; set; }
    }
}

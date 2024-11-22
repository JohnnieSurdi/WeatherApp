namespace WeatherApp.Models.Weather
{
    public class WeatherResponse
    {
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public double Temp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public double Speed { get; set; }
    }
}

using WeatherApp.Models.Weather.WeatherResponseData;

namespace WeatherApp.Models.Weather
{
    public class WeatherResponse
    {
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public Coord Coord { get; set; }
        public WeatherData Weather { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
    }
}

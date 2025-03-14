using Amazon.DynamoDBv2.DataModel;

namespace WeatherApp.Models.Database
{
    [DynamoDBTable("weather")]
    public class WeatherSearchRecord
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; }
        [DynamoDBProperty("city")]
        public string CityName { get; set; }
        [DynamoDBProperty("country")]
        public string CountryName { get; set; }
        [DynamoDBProperty("longitude")]
        public double Lon { get; set; }
        [DynamoDBProperty("latitude")]
        public double Lat { get; set; }
        [DynamoDBProperty("main")]
        public string Main { get; set; }
        [DynamoDBProperty("description")]
        public string Description { get; set; }
        [DynamoDBProperty("temp")]
        public double Temp { get; set; }
        [DynamoDBProperty("pressure")]
        public int Pressure { get; set; }
        [DynamoDBProperty("humidity")]
        public int Humidity { get; set; }
        [DynamoDBProperty("wind_speed")]
        public double Speed { get; set; }
        [DynamoDBProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

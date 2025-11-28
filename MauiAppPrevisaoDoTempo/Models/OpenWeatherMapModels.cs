
using System.Collections.Generic;
using System.Text.Json.Serialization; 

namespace MauiAppPrevisaoDoTempo.Models
{
    public class WeatherApiResult
    {
        [JsonPropertyName("name")]
        public string CityName { get; set; } = string.Empty;

        [JsonPropertyName("main")]
        public MainInfo Main { get; set; } = new MainInfo();

        [JsonPropertyName("weather")]
        public List<WeatherCondition> Weather { get; set; } = new List<WeatherCondition>();

        [JsonPropertyName("wind")]
        public object? Wind { get; set; }
    }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class WeatherCondition
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
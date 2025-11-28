using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace MauiAppPrevisaoDoTempo.Models
{
    public class Weather
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = default!;
    }

    public class MainData
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

    public class WeatherApiResult
    {
        [JsonPropertyName("name")]
        public string CityName { get; set; } = default!;

        [JsonPropertyName("main")]
        public MainData Main { get; set; } = default!;

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; } = new List<Weather>();
    }
}
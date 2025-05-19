using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Weather_SQLite_App.Models;

namespace Weather_SQLite_App.Services
{
    public class DataService
    {
        public static async Task<Weather?> GetWeather(string city) // Not GetForecast() because it will get the realtime weather, and not a future data
        {
            Weather? weather = null;

            string key = "bdb7370abc042446dd9e8453ac94b7ad";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={key}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    JObject? draft = JObject.Parse(json);

                    DateTime time = new DateTime();
                    DateTime sunrise = time.AddSeconds(draft["sys"]!["sunrise"]!.ToObject<double>()).ToLocalTime();
                    DateTime sunset = time.AddSeconds(draft["sys"]!["sunset"]!.ToObject<double>()).ToLocalTime();

                    weather = new()
                    {
                        Latitude = draft["coord"]?["lat"]?.ToObject<double>(),
                        Longitude = draft["coord"]?["lon"]?.ToObject<double>(),
                        Visibility = draft["visibility"]?.ToObject<int>(),
                        MaxTemperature = draft["main"]?["temp_max"]?.ToObject<double>(),
                        MinTemperature = draft["main"]?["temp_min"]?.ToObject<double>(),
                        Temperature = draft["main"]?["temp"]?.ToObject<double>(),
                        Sunrise = sunrise.ToString(),
                        Sunset = sunrise.ToString(),
                        Main = draft["weather"]?[0]?["main"]?.ToObject<string>(),
                        Description = draft["weather"]?[0]?["description"]?.ToObject<string>(),
                        WindSpeed = draft["wind"]?["speed"]?.ToObject<double>()
                    }; // Closes weather object
                } // Closes if statement 
            } // Closes using statement

            return weather;
        }
    }
}

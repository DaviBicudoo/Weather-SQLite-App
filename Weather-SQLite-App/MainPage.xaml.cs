using Microsoft.Maui.Platform;
using Weather_SQLite_App.Models;
using Weather_SQLite_App.Services;

namespace Weather_SQLite_App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void searchButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cityEntry.Text))
                {
                    Weather? weather = await DataService.GetWeather(cityEntry.Text);

                    if (weather != null)
                    {
                        string? weatherData = "";

                        weatherData = $"Latitude: {weather.Latitude}\n" +
                                      $"Longitude: {weather.Longitude}\n" +
                                      $"Temperature: {weather.Temperature}°C\n" +
                                      $"Max Temperature: {weather.MaxTemperature}°C\n" +
                                      $"Min Temperature: {weather.MinTemperature}%\n" +
                                      $"Main: {weather.Main} hPa\n" +
                                      $"Description: {weather.Description} m/s\n" +
                                      $"Sunrise: {weather.Sunrise}\n" +
                                      $"Sunset: {weather.Sunset}\n" +
                                      $"Visibility: {weather.Visibility}\n" +
                                      $"Wind Speed: {weather.WindSpeed} m/s\n";

                        responseLabel.Text = weatherData;

                        string map = $"https://embed.windy.com/embed.html?" +
                            $"type=map&location=coordinates&metricRain=mm" +
                            $"&metricTemp=°C&metricWind=km/h&zoom=10&overlay=wind&product=ecmwf&level=surface" +
                            $"&lat={weather.Latitude.ToString()?.Replace(",", ".")}&lon={weather.Longitude.ToString()?.Replace(",", ".")}" +
                        $"&marker=true";
                    }
                    else
                    {
                        responseLabel.Text = "No forecast data.";
                    }
                }
                else
                {
                    responseLabel.Text = "Please enter a city name.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}

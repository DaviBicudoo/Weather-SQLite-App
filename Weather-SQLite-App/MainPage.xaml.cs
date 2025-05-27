using System.Collections.ObjectModel;
using Microsoft.Maui.Platform;
using Weather_SQLite_App.Models;
using Weather_SQLite_App.Services;

namespace Weather_SQLite_App
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Weather> list = new ObservableCollection<Weather>();

        public MainPage()
        {
            InitializeComponent();

            OnAppearing();

            weatherList.ItemsSource = list;
        }

        protected async override void OnAppearing()
        {
            try
            {
                list.Clear(); // It clears the list view every time we go back to the window

                List<Weather> temporaryList = await App.Database.GetAll();

                temporaryList.ForEach(x => list.Add(x));

            }
            catch (Exception ex)
            {
                await DisplayAlert("OPS!", ex.Message, "OK");
            }
        }

        private async void searchButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cityEntry.Text))
                {
                    Weather? weather = await DataService.GetWeather(cityEntry.Text);

                    weather.Name = cityEntry.Text;

                    if (weather != null)
                    {
                        string? weatherData = "";

                        weatherData = $"Name: {weather.Name} \n" +
                                      $"Latitude: {weather.Latitude}\n" +
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

                        await App.Database.Create(weather);
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

        private void weatherList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem? selectedMenuItem = sender as MenuItem;

                Weather? weather = selectedMenuItem?.BindingContext as Weather;

                bool confirm = await DisplayAlert("Warning!", $"Do you really want to exclude this product? ({weather.Description})",
                    "Yes", "No");

                if (confirm)
                {
                    await App.Database.Delete(weather.Id);
                    list.Remove(weather);
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("OPS!", ex.Message, "OK");
            }
        }

        private void reloadTable_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            Navigation.PushAsync(new MainPage());
        }
    }
}

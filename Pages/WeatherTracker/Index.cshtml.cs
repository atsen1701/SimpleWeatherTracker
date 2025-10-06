using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SideProject_C__.Pages.WeatherTracker
{
    public class WeatherModel : PageModel
    {
        public string city { get; set; } = "Kuala Lumpur";
        public string? weatherDescription { get; set; }
        public string? temperature { get; set; }
        public string? errorMessage { get; set; }

        public async Task OnGetAsync()
        {
            await FetchWeatherInformation();
        }

        private async Task FetchWeatherInformation()
        {
            try
            {
                var apiKey = "a3c456c241837a94545d8785c55465b5";
                var apiUrl =
                    $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using var weatherData = JsonDocument.Parse(content);
                    var main = weatherData.RootElement.GetProperty("main");
                    var weather = weatherData.RootElement.GetProperty("weather")[0];

                    temperature = main.GetProperty("temp").GetDouble().ToString("F1");
                    weatherDescription = weather.GetProperty("description").GetString();
                }
                else
                {
                    errorMessage = $"API Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
            }
        }
    }
}

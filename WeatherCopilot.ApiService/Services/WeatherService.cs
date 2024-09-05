using System.Text.Json;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ForecastResponse[]> CallWeatherServiceAsync(double latitude, double longitude)
    {
        string pointUrl = $"https://api.weather.gov/points/{latitude:N2},{longitude:N2}";

        HttpResponseMessage response = await _httpClient.GetAsync(pointUrl);
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            WeatherPoint weatherPoint = JsonSerializer.Deserialize<WeatherPoint>(content);

            if (weatherPoint.Properties.Forecast != null)
            {
                string forecastUrl = weatherPoint.Properties.Forecast;
                response = await _httpClient.GetAsync(forecastUrl);

                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    WeatherForecast dailyForecast = JsonSerializer.Deserialize<WeatherForecast>(content);

                    var forecasts = new List<ForecastResponse>();
                    foreach (var period in dailyForecast.Properties.Periods)
                    {
                        var dayForecast = new ForecastResponse(
                            Order: period.Number,
                            Name: period.Name,
                            TemperatureF: period.Temperature,
                            Summary: period.ShortForecast,
                            Details: period.DetailedForecast
                        );
                        forecasts.Add(dayForecast);
                    }

                    return forecasts.ToArray();
                }
            }
        }
        // Handle the error appropriately  
        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        return Array.Empty<ForecastResponse>();
    }

    public record ForecastResponse(int Order, string Name, int TemperatureF, string Summary, string Details)
    {
        public int TemperatureC => (int)((TemperatureF - 32) * (5.0 / 9.0));
    }
}

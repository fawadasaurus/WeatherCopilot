using System.Text.Json;

public class LocationService
{
    private readonly HttpClient _httpClient;

    public LocationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "WeatherCopilot");
    }

    public async Task<GeoLocation?> GetGeoLocationAsync(string city, string state)
    {
        var baseUrl = "https://nominatim.openstreetmap.org/search.php";
        var query = $"?city={Uri.EscapeDataString(city)}&state={Uri.EscapeDataString(state)}&country=USA&format=jsonv2";

        var response = await _httpClient.GetAsync(baseUrl + query);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var locations = JsonSerializer.Deserialize<GeoLocation[]>(content);
            return locations?.Length > 0 ? locations[0] : null;
        }

        return null;
    }
}

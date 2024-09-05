using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherCopilot.Controllers
{
    public class WebForecast
    {
        public async Task<IEnumerable<WeatherService.ForecastResponse>> GetForecastsAsync(string city, string state)
        {
            var httpClient = new HttpClient();
            var locationService = new LocationService(httpClient);
            var geoLocation = await locationService.GetGeoLocationAsync("Atlanta", "Georgia");

            var latitude = double.Parse(geoLocation?.Latitude ?? "0.0");
            var longitude = double.Parse(geoLocation?.Longitude ?? "0.0");

            var weatherService = new WeatherService(httpClient);
            return await weatherService.CallWeatherServiceAsync(latitude, longitude);
        }
    }
}

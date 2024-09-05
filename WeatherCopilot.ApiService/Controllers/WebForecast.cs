namespace WeatherCopilot.Controllers
{
    public class WebForecast
    {
        private readonly LocationService _locationService;
        private readonly WeatherService _weatherService;

        public WebForecast(LocationService locationService, WeatherService weatherService, AIService aiService)
        {
            _locationService = locationService;
            _weatherService = weatherService;
            _aiService = aiService;
        }

        public async Task<IEnumerable<WeatherService.ForecastResponse>> GetForecastsAsync(string city, string state)
        {
            var geoLocation = await _locationService.GetGeoLocationAsync(city, state);

            if (geoLocation == null)
            {
                return Enumerable.Empty<WeatherService.ForecastResponse>();
            }

            var latitude = double.Parse(geoLocation?.Latitude ?? "0.0");
            var longitude = double.Parse(geoLocation?.Longitude ?? "0.0");

            return await _weatherService.CallWeatherServiceAsync(latitude, longitude);
        }
    }
}

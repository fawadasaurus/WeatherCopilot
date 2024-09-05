// Controllers/WebForecast.cs

namespace WeatherCopilot.Controllers
{
    public class WebForecast
    {
        private readonly ForecastService _forecastService;
        private readonly AIService _aiService;

        public WebForecast(ForecastService forecastService, AIService aiService)
        {
            _forecastService = forecastService;
            _aiService = aiService;
        }

        public async Task<IEnumerable<WeatherService.ForecastResponse>> GetForecastsAsync(string city, string state)
        {
            return await _forecastService.GetForecastsAsync(city, state);
        }

        public async Task<ChatMessage> GetChatResponseAsync(ChatMessage chatPrompt)
        {
            return await _aiService.CompleteChatAsync(chatPrompt.Message);
        }

        public record ChatMessage(string Message);
    }
}

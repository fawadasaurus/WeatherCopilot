// Services/AIService.cs

using System.Text;
using System.Text.Json;
using OpenAI.Chat;
public class AIService
{
    private readonly ChatClient _chatClient;
    private readonly ForecastService _forecastService;

    public AIService(ChatClient chatClient, ForecastService forecastService)
    {
        _chatClient = chatClient;
        _forecastService = forecastService;
    }

    public async Task<string> CompleteChatAsync(string prompt)
    {
        List<ChatMessage> conversationMessages = new List<ChatMessage> {  
            // System messages represent instructions or other guidance about how the assistant should behave  
            new SystemChatMessage("You are an assistant that helps people answer questions using details of the weather in their location. (City and State). You are limited to American cities only."),  
            // User messages represent user input, whether historical or the most recent input  
            new UserChatMessage(prompt)
        };

        ChatTool getWeatherForecastTool = ChatTool.CreateFunctionTool(
            functionName: nameof(_forecastService.GetForecastsAsync),
            functionDescription: "Get the current and upcoming weather forecast for a given city and state",
            functionParameters: BinaryData.FromString("""
                {
                    "type": "object",
                    "properties": {
                        "city": {
                            "type": "string",
                            "description": "The city to get the weather for. eg: Miami or New York"
                        },
                        "state": {
                            "type": "string",
                            "description": "The state the city is in. eg: FL or Florida or NY or New York"
                        }
                    },
                    "required": [ "city", "state" ]
                }
                """)
        );

        ChatCompletionOptions options = new()
        {
            Tools = { getWeatherForecastTool },
        };

        ChatCompletion completion = _chatClient.CompleteChat(conversationMessages, options);

        if (completion.ToolCalls.Count > 0)
        {
            // Important to add the completion to the conversation messages so next completion can be aware of the previous completion
            conversationMessages.Add(new AssistantChatMessage(completion));

            foreach (var toolCall in completion.ToolCalls)
            {
                if (toolCall.FunctionName == nameof(_forecastService.GetForecastsAsync))
                {
                    using JsonDocument argumentsDocument = JsonDocument.Parse(toolCall.FunctionArguments);
                    if (!argumentsDocument.RootElement.TryGetProperty("city", out JsonElement cityElement) || !argumentsDocument.RootElement.TryGetProperty("state", out JsonElement stateElement))
                    {
                        return "Please provide a city and state to get the weather forecast.";
                    }

                    var city = cityElement.GetString() ?? "New York";
                    var state = stateElement.GetString() ?? "NY";

                    var forecasts = await _forecastService.GetForecastsAsync(city, state);

                    var forecastString = new StringBuilder();
                    foreach (var forecast in forecasts)
                    {
                        forecastString.AppendLine(forecast.ToString());
                    }

                    // Add the concatenated forecast string as a single ToolChatMessage  
                    conversationMessages.Add(new ToolChatMessage(toolCall.Id, forecastString.ToString()));
                }
            }
        }
        else
        {
            return "Please provide a city and state to get the weather forecast.";
        }
        ChatCompletion finalCompletion = _chatClient.CompleteChat(conversationMessages, options);

        return finalCompletion.Content.FirstOrDefault()?.Text ?? "I'm sorry, I don't have that information.";
    }
}


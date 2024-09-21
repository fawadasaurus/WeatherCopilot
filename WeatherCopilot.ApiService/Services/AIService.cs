// Services/AIService.cs  
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenAI.Chat;
using WeatherCopilot.Controllers;
using Microsoft.Extensions.Logging;

public class AIService
{
    private readonly ChatClient _chatClient;
    private readonly ForecastService _forecastService;
    private readonly ILogger<AIService> _logger;

    public AIService(ChatClient chatClient, ForecastService forecastService, ILogger<AIService> logger)
    {
        _chatClient = chatClient;
        _forecastService = forecastService;
        _logger = logger;
    }

    public async Task<WebForecast.ChatMessage> CompleteChatAsync(string prompt, bool useTool = false)
    {
        List<OpenAI.Chat.ChatMessage> conversationMessages = new List<OpenAI.Chat.ChatMessage>
        {
            new SystemChatMessage(@"You are an assistant that helps people answer questions using details of the weather in their location. (City and State).  
            You are limited to American cities only. Keep your responses clear and concise."),
            new UserChatMessage(prompt)
        };

        LogObject("Initial Conversation Messages", conversationMessages);

        ChatTool getWeatherForecastTool = ChatTool.CreateFunctionTool(
            functionName: nameof(_forecastService.GetForecastsAsync),
            functionDescription: "Get the current and upcoming weather forecast for a given city and state",
            functionParameters: BinaryData.FromString(@"  
            {  
                ""type"": ""object"",  
                ""properties"": {  
                    ""city"": {  
                        ""type"": ""string"",  
                        ""description"": ""The city to get the weather for. eg: Miami or New York""  
                    },  
                    ""state"": {  
                        ""type"": ""string"",  
                        ""description"": ""The state the city is in. eg: FL or Florida or NY or New York""  
                    }  
                },  
                ""required"": [ ""city"", ""state"" ]  
            }  
            ")
        );

        ChatCompletionOptions options = new()
        {
            Tools = { },
        };

        if (useTool)
        {
            options.Tools.Add(getWeatherForecastTool);
        }

        LogObject("Chat Completion Options", options);

        ChatCompletion completion = _chatClient.CompleteChat(conversationMessages, options);

        LogObject("Initial Chat Completion Response", completion);

        if (completion.ToolCalls.Count > 0)
        {
            // This is very important. If you don't add the completion to the conversation messages, 
            // OpenAI will not be able to know which tools calls were made and will reject the next prompt.
            conversationMessages.Add(new AssistantChatMessage(completion));
            LogObject("Conversation Messages", conversationMessages);

            foreach (var toolCall in completion.ToolCalls)
            {
                if (toolCall.FunctionName == nameof(_forecastService.GetForecastsAsync))
                {
                    using JsonDocument argumentsDocument = JsonDocument.Parse(toolCall.FunctionArguments);

                    if (!argumentsDocument.RootElement.TryGetProperty("city", out JsonElement cityElement)
                        || !argumentsDocument.RootElement.TryGetProperty("state", out JsonElement stateElement))
                    {
                        return new WebForecast.ChatMessage("Please provide a city and state to get the weather forecast.");
                    }

                    var city = cityElement.GetString() ?? "New York";
                    var state = stateElement.GetString() ?? "NY";

                    var forecasts = await _forecastService.GetForecastsAsync(city, state);

                    var forecastString = new StringBuilder();
                    foreach (var forecast in forecasts)
                    {
                        forecastString.AppendLine(forecast.ToString());
                    }

                    conversationMessages.Add(new ToolChatMessage(toolCall.Id, forecastString.ToString()));
                    LogObject("Conversation Messages", conversationMessages);
                }
            }

            ChatCompletion finalCompletion = _chatClient.CompleteChat(conversationMessages, options);

            LogObject("Final Chat Completion", finalCompletion);

            return new WebForecast.ChatMessage(finalCompletion.Content.FirstOrDefault()?.Text ?? "I'm sorry, I don't have that information.");
        }
        else
        {
            return new WebForecast.ChatMessage(completion.Content.FirstOrDefault()?.Text ?? "I'm sorry, I don't have that information.");
        }
    }

    private void LogObject(string name, object obj)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };
        var json = JsonSerializer.Serialize(obj, options);
        _logger.LogInformation("{Name}:\n {Json}", name, json);
    }

}

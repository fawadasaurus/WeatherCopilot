// AIService.cs  
using OpenAI.Chat;

public class AIService
{
    private readonly ChatClient _chatClient;

    public AIService(ChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<ChatCompletion> CompleteChatAsync(ChatThread thread)
    {
        return await _chatClient.CompleteChatAsync(thread);
    }
}

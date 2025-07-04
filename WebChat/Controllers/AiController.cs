using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace WebChat.Controllers;

[Route("api/[controller]")]
public class AiController : ControllerBase
{
    [HttpGet]
    public async IAsyncEnumerable<ChatResponseUpdate> GetChatAsync()
    {
        using var ollamaClient = new OllamaChatClient("http://localhost:11434","deepseek-r1:1.5b");
        var messageList = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System,  "你是一个诗人."),
            new ChatMessage(ChatRole.User, "请写一首关于机器的诗.")
        };
        var result = ollamaClient.GetStreamingResponseAsync(messageList);
        await foreach (var item in result)
        {
            yield return item;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace WebChat.Controllers;

[Route("api/[controller]")]
public class AiController : ControllerBase
{
    [HttpPost]
    public async IAsyncEnumerable<ChatResponseUpdate> GetChatAsync([FromBody] ChatDto chatDto)
    {
        using var ollamaClient = new OllamaChatClient("http://localhost:11434","deepseek-r1:1.5b");
        var messageList = chatDto.ChatItemDtos.Select(x => new ChatMessage(new ChatRole(x.Role), x.Content)).ToList();
        var result = ollamaClient.GetStreamingResponseAsync(messageList);
        await foreach (var item in result)
        {
            yield return item;
        }
    }

    public class ChatDto
    {
        public List<ChatItemDto> ChatItemDtos { get; set; }
    }
    
    public class ChatItemDto
    {
        public string Role { get; set; }
        
        public string Content { get; set; }
    }
}
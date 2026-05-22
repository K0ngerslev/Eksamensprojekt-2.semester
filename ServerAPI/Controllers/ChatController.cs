using Microsoft.AspNetCore.Mvc;
using ServerAPI.Chat;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly ChatRepository _repository;

    public ChatController(ChatRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<ChatRepository.ChatMessage>>> GetMessages()
    {
        var messages = await _repository.GetMessagesAsync();
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendChatMessageDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.User) || string.IsNullOrWhiteSpace(dto.Text))
            return BadRequest("Bruger og besked skal udfyldes");

        await _repository.AddMessageAsync(dto.User, dto.Text);
        return Ok();
    }
}

public class SendChatMessageDto
{
    public string User { get; set; } = "";
    public string Text { get; set; } = "";
}
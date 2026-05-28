using Microsoft.AspNetCore.Mvc;
using Core.Model;
using ServerAPI.Repositories;

[ApiController]
[Route("api/chat")]
public class TeamChatController : ControllerBase
{
    private readonly ITeamchatRepository repository;

    public TeamChatController(ITeamchatRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<TeamChatModel>>> GetMessages()
    {
        var messages = await repository.GetMessagesAsync();
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendChatMessageDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.User) || string.IsNullOrWhiteSpace(dto.Text))
            return BadRequest("Bruger og besked skal udfyldes");

        await repository.AddMessageAsync(dto.User, dto.Text);
        return Ok();
    }
}

public class SendChatMessageDto
{
    public string User { get; set; } = "";
    public string Text { get; set; } = "";
}
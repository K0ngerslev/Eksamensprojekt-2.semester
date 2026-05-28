using Core.Model;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeltagerController : ControllerBase
{
    private readonly IDeltagerRepository repo;

    public DeltagerController(IDeltagerRepository repo) => this.repo = repo;
    
    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] DeltagerSvar deltagerSvar)
    {
        if (string.IsNullOrWhiteSpace(deltagerSvar.AktivitetId) ||
            string.IsNullOrWhiteSpace(deltagerSvar.UserName))
            return BadRequest("AktivitetId og UserName er påkrævet.");

        await repo.UpsertAsync(deltagerSvar);
        return NoContent();
    }
}
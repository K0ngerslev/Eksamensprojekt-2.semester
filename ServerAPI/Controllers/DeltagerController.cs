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

    [HttpGet("{aktivitetId}")]
    public async Task<ActionResult<List<DeltagerSvar>>> GetByAktivitetId(string aktivitetId)
    {
        if (string.IsNullOrWhiteSpace(aktivitetId))
            return BadRequest(new { detail = "AktivitetId er påkrævet." });

        var deltagere = await repo.GetByAktivitetIdAsync(aktivitetId);
        return Ok(deltagere);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] DeltagerSvar deltagerSvar)
    {
        if (string.IsNullOrWhiteSpace(deltagerSvar.AktivitetId) ||
            string.IsNullOrWhiteSpace(deltagerSvar.UserName))
            return BadRequest(new { detail = "AktivitetId og UserName er påkrævet." });

        await repo.UpsertAsync(deltagerSvar);
        return NoContent();
    }
}
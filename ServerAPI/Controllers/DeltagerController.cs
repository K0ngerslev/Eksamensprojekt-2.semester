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
    public async Task<ActionResult<List<DeltagerSvar>>> GetByAktivitet(string aktivitetId) =>
        Ok(await repo.GetByAktivitetIdAsync(aktivitetId));

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
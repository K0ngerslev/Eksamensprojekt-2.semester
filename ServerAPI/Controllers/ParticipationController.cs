using Core.Model;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipationController : ControllerBase
{
    private readonly IParticipationRepository _repo;

    public ParticipationController(IParticipationRepository repo) => _repo = repo;

    [HttpGet("{aktivitetId}")]
    public async Task<ActionResult<List<Participation>>> GetByAktivitet(string aktivitetId) =>
        Ok(await _repo.GetByAktivitetIdAsync(aktivitetId));

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] Participation participation)
    {
        if (string.IsNullOrWhiteSpace(participation.AktivitetId) ||
            string.IsNullOrWhiteSpace(participation.UserName))
            return BadRequest("AktivitetId og UserName er påkrævet.");

        await _repo.UpsertAsync(participation);
        return NoContent();
    }
}
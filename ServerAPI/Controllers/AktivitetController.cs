using Core.Model;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AktivitetController : ControllerBase
{
    private readonly IAktivitet _repo;

    public AktivitetController(IAktivitet repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<List<Aktivitet>>> GetAll()
    {
        var aktiviteter = await _repo.GetAllAsync();
        return Ok(aktiviteter);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Aktivitet>> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id is required.");
        }

        var aktivitet = await _repo.GetByIdAsync(id);

        if (aktivitet is null)
        {
            return NotFound();
        }

        return Ok(aktivitet);
    }

    [HttpPost]
    public async Task<ActionResult<Aktivitet>> Create([FromBody] Aktivitet aktivitet)
    {
        var validationResult = ValidateAktivitet(aktivitet);
        if (validationResult is not null)
        {
            return validationResult;
        }

        aktivitet.Id = null;
        await _repo.CreateAsync(aktivitet);

        return CreatedAtAction(nameof(GetById), new { id = aktivitet.Id }, aktivitet);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Aktivitet aktivitet)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id is required.");
        }

        if (await _repo.GetByIdAsync(id) is null)
        {
            return NotFound();
        }

        var validationResult = ValidateAktivitet(aktivitet);
        if (validationResult is not null)
        {
            return validationResult;
        }

        await _repo.UpdateAsync(id, aktivitet);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id is required.");
        }

        if (await _repo.GetByIdAsync(id) is null)
        {
            return NotFound();
        }

        await _repo.DeleteAsync(id);
        return NoContent();
    }

    private ActionResult? ValidateAktivitet(Aktivitet aktivitet)
    {
        NormalizeAktivitet(aktivitet);

        if (string.IsNullOrWhiteSpace(aktivitet.ActivityType))
        {
            ModelState.AddModelError(nameof(aktivitet.ActivityType), "ActivityType is required.");
        }
        else if (aktivitet.ActivityType.Length is < 2 or > 50)
        {
            ModelState.AddModelError(nameof(aktivitet.ActivityType), "ActivityType must be between 2 and 50 characters.");
        }

        if (aktivitet.Date is null)
        {
            ModelState.AddModelError(nameof(aktivitet.Date), "Date is required.");
        }

        if (aktivitet.Time is null)
        {
            ModelState.AddModelError(nameof(aktivitet.Time), "Time is required.");
        }

        if (string.IsNullOrWhiteSpace(aktivitet.FieldOrLocation))
        {
            ModelState.AddModelError(nameof(aktivitet.FieldOrLocation), "FieldOrLocation is required.");
        }
        else if (aktivitet.FieldOrLocation.Length is < 2 or > 100)
        {
            ModelState.AddModelError(nameof(aktivitet.FieldOrLocation), "FieldOrLocation must be between 2 and 100 characters.");
        }

        if (aktivitet.ChangingRoom?.Length > 100)
        {
            ModelState.AddModelError(nameof(aktivitet.ChangingRoom), "ChangingRoom cannot be longer than 100 characters.");
        }

        if (aktivitet.AdditionalNotes?.Length > 500)
        {
            ModelState.AddModelError(nameof(aktivitet.AdditionalNotes), "AdditionalNotes cannot be longer than 500 characters.");
        }

        return ModelState.IsValid ? null : ValidationProblem(ModelState);
    }

    private static void NormalizeAktivitet(Aktivitet aktivitet)
    {
        aktivitet.ActivityType = aktivitet.ActivityType?.Trim();
        aktivitet.FieldOrLocation = aktivitet.FieldOrLocation?.Trim();
        aktivitet.ChangingRoom = string.IsNullOrWhiteSpace(aktivitet.ChangingRoom) ? null : aktivitet.ChangingRoom.Trim();
        aktivitet.AdditionalNotes = string.IsNullOrWhiteSpace(aktivitet.AdditionalNotes) ? null : aktivitet.AdditionalNotes.Trim();
    }
}

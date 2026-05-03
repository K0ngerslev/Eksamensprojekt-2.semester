using Core.Model;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

// Markerer klassen som en API-controller og giver den ruten api/aktivitet.
[ApiController]
[Route("api/[controller]")]
public class AktivitetController : ControllerBase
{
    // Repository bruges til at hente og gemme aktiviteter i databasen.
    private readonly IAktivitet _repo;

    // Constructor injicerer repositoryet, så controlleren kan bruge det i endpoints.
    public AktivitetController(IAktivitet repo)
    {
        _repo = repo;
    }

    // Henter alle aktiviteter fra databasen og returnerer dem som et svar.
    [HttpGet]
    public async Task<ActionResult<List<Aktivitet>>> GetAll()
    {
        var aktiviteter = await _repo.GetAllAsync();
        return Ok(aktiviteter);
    }

    // Henter en enkelt aktivitet ud fra dens id.
    [HttpGet("{id}")]
    public async Task<ActionResult<Aktivitet>> GetById(string id)
    {
        // Stopper requesten, hvis id mangler eller kun indeholder mellemrum.
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id is required.");
        }

        // Forsøger at finde aktiviteten i databasen.
        var aktivitet = await _repo.GetByIdAsync(id);

        // Returnerer 404 hvis aktiviteten ikke findes.
        if (aktivitet is null)
        {
            return NotFound();
        }

        // Returnerer aktiviteten hvis den blev fundet.
        return Ok(aktivitet);
    }

    // Opretter en ny aktivitet ud fra data sendt i request body.
    [HttpPost]
    public async Task<ActionResult<Aktivitet>> Create([FromBody] Aktivitet aktivitet)
    {
        // Validerer input før aktiviteten gemmes.
        var validationResult = ValidateAktivitet(aktivitet);
        if (validationResult is not null)
        {
            return validationResult;
        }

        // Sørger for at databasen selv opretter et nyt id.
        aktivitet.Id = null;
        await _repo.CreateAsync(aktivitet);

        // Returnerer 201 Created med reference til endpointet, der kan hente aktiviteten.
        return CreatedAtAction(nameof(GetById), new { id = aktivitet.Id }, aktivitet);
    }

    // Opdaterer en eksisterende aktivitet ud fra id og nye værdier.
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Aktivitet aktivitet)
    {
        // Stopper requesten, hvis id mangler eller kun indeholder mellemrum.
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id is required.");
        }

        // Tjekker først om aktiviteten findes, før den forsøges opdateret.
        if (await _repo.GetByIdAsync(id) is null)
        {
            return NotFound();
        }

        // Validerer de nye værdier inden opdatering.
        var validationResult = ValidateAktivitet(aktivitet);
        if (validationResult is not null)
        {
            return validationResult;
        }

        // Gemmer de opdaterede data på den eksisterende aktivitet.
        await _repo.UpdateAsync(id, aktivitet);
        return NoContent();
    }

    // Sletter en aktivitet ud fra dens id.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        // Stopper requesten, hvis id mangler eller kun indeholder mellemrum.
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id is required.");
        }

        // Tjekker om aktiviteten findes, før den slettes.
        if (await _repo.GetByIdAsync(id) is null)
        {
            return NotFound();
        }

        // Sletter aktiviteten og returnerer et tomt succes-svar.
        await _repo.DeleteAsync(id);
        return NoContent();
    }

    // Samler al validering af aktivitetens felter i controlleren.
    private ActionResult? ValidateAktivitet(Aktivitet aktivitet)
    {
        // Rydder input op, så unødige mellemrum ikke giver dårlige data.
        NormalizeAktivitet(aktivitet);

        // Tjekker at aktivitetstypen er udfyldt.
        if (string.IsNullOrWhiteSpace(aktivitet.ActivityType))
        {
            ModelState.AddModelError(nameof(aktivitet.ActivityType), "Aktivitetstype mangler.");
        }
        // Tjekker at aktivitetstypen holder sig inden for den tilladte længde.
        else if (aktivitet.ActivityType.Length is < 2 or > 50)
        {
            ModelState.AddModelError(nameof(aktivitet.ActivityType), "Activitetstypen skal være mellem 2 og 50 tegn.");
        }

        // Tjekker at der er valgt en dato.
        if (aktivitet.Date is null)
        {
            ModelState.AddModelError(nameof(aktivitet.Date), "Date mangler.");
        }

        // Tjekker at der er valgt et tidspunkt.
        if (aktivitet.Time is null)
        {
            ModelState.AddModelError(nameof(aktivitet.Time), "Tidspunkt mangler.");
        }

        // Tjekker at sted/bane er udfyldt.
        if (string.IsNullOrWhiteSpace(aktivitet.FieldOrLocation))
        {
            ModelState.AddModelError(nameof(aktivitet.FieldOrLocation), "Bane mangler");
        }

        // Tjekker at omklædningsrum ikke er for langt.
        if (aktivitet.ChangingRoom?.Length > 100)
        {
            ModelState.AddModelError(nameof(aktivitet.ChangingRoom), "Omklædningrum må ikke fylde mere end 100 tegn.");
        }

        // Tjekker at ekstra noter ikke er for lange.
        if (aktivitet.AdditionalNotes?.Length > 500)
        {
            ModelState.AddModelError(nameof(aktivitet.AdditionalNotes), "Noter må ikke være mere end 500 tegn.");
        }

        // Returnerer enten null ved gyldig model eller en samlet valideringsfejl.
        return ModelState.IsValid ? null : ValidationProblem(ModelState);
    }

    // Renser tekstfelter ved at trimme mellemrum og sætte tomme valgfrie felter til null.
    private static void NormalizeAktivitet(Aktivitet aktivitet)
    {
        aktivitet.ActivityType = aktivitet.ActivityType?.Trim();
        aktivitet.FieldOrLocation = aktivitet.FieldOrLocation?.Trim();
        aktivitet.ChangingRoom = string.IsNullOrWhiteSpace(aktivitet.ChangingRoom) ? null : aktivitet.ChangingRoom.Trim();
        aktivitet.AdditionalNotes = string.IsNullOrWhiteSpace(aktivitet.AdditionalNotes) ? null : aktivitet.AdditionalNotes.Trim();
    }
}

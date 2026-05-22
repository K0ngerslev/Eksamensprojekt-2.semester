using System.Globalization;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Repositories;

namespace ServerAPI.Controllers;

// Markerer klassen som en API-controller og giver den ruten api/aktivitet.
[ApiController]
[Route("api/[controller]")]
public class AktivitetController : ControllerBase
{
    private static readonly string[] SupportedDateFormats = ["yyyy-MM-dd"];
    private static readonly string[] SupportedTimeFormats = ["HH:mm", "HH:mm:ss"];

    // Repository bruges til at hente og gemme aktiviteter i databasen.
    private readonly IAktivitetRepository repo;

    // Constructor injicerer repositoryet, så controlleren kan bruge det i endpoints.
    public AktivitetController(IAktivitetRepository repo)
    {
        this.repo = repo;
    }

    // Henter alle aktiviteter fra databasen og returnerer dem som et svar.
    [HttpGet]
    public async Task<ActionResult<List<AktivitetModel>>> GetAll()
    {
        var aktiviteter = await repo.GetAllAsync();
        return Ok(aktiviteter);
    }

    // Henter en enkelt aktivitet ud fra dens id.
    [HttpGet("{id}")]
    public async Task<ActionResult<AktivitetModel>> GetById(string id)
    {
        // Stopper requesten, hvis id mangler eller kun indeholder mellemrum.
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id is required.");
        }

        // Forsøger at finde aktiviteten i databasen.
        var aktivitet = await repo.GetByIdAsync(id);

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
    public async Task<ActionResult<AktivitetModel>> Create([FromBody] AktivitetRequest request)
    {
        // Validerer input før aktiviteten gemmes.
        var aktivitet = BuildAktivitet(request);
        var validationResult = ValidateAktivitet(aktivitet, request);
        if (validationResult is not null)
        {
            return validationResult;
        }

        // Sørger for at databasen selv opretter et nyt id.
        aktivitet.Id = null;
        await repo.CreateAsync(aktivitet);

        // Returnerer 201 Created med reference til endpointet, der kan hente aktiviteten.
        return CreatedAtAction(nameof(GetById), new { id = aktivitet.Id }, aktivitet);
    }

    // Opdaterer en eksisterende aktivitet ud fra id og nye værdier.
    [HttpPut("{id}")]
    public async Task<ActionResult<AktivitetModel>> Update(string id, [FromBody] AktivitetRequest request)
    {
        // Stopper requesten, hvis id mangler eller kun indeholder mellemrum.
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id is required.");
        }

        // Tjekker først om aktiviteten findes, før den forsøges opdateret.
        if (await repo.GetByIdAsync(id) is null)
        {
            return NotFound();
        }

        // Validerer de nye værdier inden opdatering.
        var aktivitet = BuildAktivitet(request);
        var validationResult = ValidateAktivitet(aktivitet, request);
        if (validationResult is not null)
        {
            return validationResult;
        }

        // Gemmer de opdaterede data på den eksisterende aktivitet.
        await repo.UpdateAsync(id, aktivitet);
        return Ok(aktivitet);
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
        if (await repo.GetByIdAsync(id) is null)
        {
            return NotFound();
        }

        // Sletter aktiviteten og returnerer et tomt succes-svar.
        await repo.DeleteAsync(id);
        return NoContent();
    }

    // Samler al validering af aktivitetens felter i controlleren.
    private ActionResult? ValidateAktivitet(AktivitetModel aktivitetModel, AktivitetRequest request)
    {
        // Rydder input op, så unødige mellemrum ikke giver dårlige data.
        NormalizeAktivitet(aktivitetModel);

        // Tjekker at aktivitetstypen er udfyldt.
        if (string.IsNullOrWhiteSpace(aktivitetModel.ActivityType))
        {
            ModelState.AddModelError(nameof(aktivitetModel.ActivityType), "Aktivitetstype mangler.");
        }
        // Tjekker at aktivitetstypen holder sig inden for den tilladte længde.
        else if (aktivitetModel.ActivityType.Length is < 2 or > 50)
        {
            ModelState.AddModelError(nameof(aktivitetModel.ActivityType), "Activitetstypen skal være mellem 2 og 50 tegn.");
        }

        // Tjekker at der er valgt en dato.
        if (string.IsNullOrWhiteSpace(request.Date))
        {
            ModelState.AddModelError(nameof(aktivitetModel.Date), "Dato mangler.");
        }
        else if (aktivitetModel.Date is null)
        {
            ModelState.AddModelError(nameof(aktivitetModel.Date), "Dato er ugyldig.");
        }

        // Tjekker at der er valgt et tidspunkt.
        if (string.IsNullOrWhiteSpace(request.StartTime))
        {
            ModelState.AddModelError(nameof(aktivitetModel.StartTime), "Starttid mangler.");
        }
        else if (aktivitetModel.StartTime is null)
        {
            ModelState.AddModelError(nameof(aktivitetModel.StartTime), "Starttid er ugyldig.");
        }

        if (string.IsNullOrWhiteSpace(request.EndTime))
        {
            ModelState.AddModelError(nameof(aktivitetModel.EndTime), "Sluttid mangler.");
        }
        else if (aktivitetModel.EndTime is null)
        {
            ModelState.AddModelError(nameof(aktivitetModel.EndTime), "Sluttid er ugyldig.");
        }
        else if (aktivitetModel.StartTime is not null && aktivitetModel.EndTime <= aktivitetModel.StartTime)
        {
            ModelState.AddModelError(nameof(aktivitetModel.EndTime), "Sluttid skal ligge efter starttid.");
        }

        // Tjekker at sted/bane er udfyldt.
        if (string.IsNullOrWhiteSpace(aktivitetModel.FieldOrLocation))
        {
            ModelState.AddModelError(nameof(aktivitetModel.FieldOrLocation), "Bane mangler");
        }

        // Tjekker at omklædningsrum ikke er for langt.
        if (aktivitetModel.ChangingRoom?.Length > 100)
        {
            ModelState.AddModelError(nameof(aktivitetModel.ChangingRoom), "Omklædningrum må ikke fylde mere end 100 tegn.");
        }

        // Tjekker at ekstra noter ikke er for lange.
        if (aktivitetModel.AdditionalNotes?.Length > 500)
        {
            ModelState.AddModelError(nameof(aktivitetModel.AdditionalNotes), "Noter må ikke være mere end 500 tegn.");
        }

        // Returnerer enten null ved gyldig model eller en samlet valideringsfejl.
        return ModelState.IsValid ? null : ValidationProblem(ModelState);
    }

    private static AktivitetModel BuildAktivitet(AktivitetRequest request)
    {
        return new AktivitetModel
        {
            ActivityType = request.ActivityType,
            Date = ParseDate(request.Date),
            StartTime = ParseTime(request.StartTime),
            EndTime = ParseTime(request.EndTime),
            FieldOrLocation = request.FieldOrLocation,
            ChangingRoom = request.ChangingRoom,
            AdditionalNotes = request.AdditionalNotes
        };
    }

    // Renser tekstfelter ved at trimme mellemrum og sætte tomme valgfrie felter til null.
    private static void NormalizeAktivitet(AktivitetModel aktivitetModel)
    {
        aktivitetModel.ActivityType = aktivitetModel.ActivityType?.Trim();
        aktivitetModel.FieldOrLocation = aktivitetModel.FieldOrLocation?.Trim();
        aktivitetModel.ChangingRoom = string.IsNullOrWhiteSpace(aktivitetModel.ChangingRoom) ? null : aktivitetModel.ChangingRoom.Trim();
        aktivitetModel.AdditionalNotes = string.IsNullOrWhiteSpace(aktivitetModel.AdditionalNotes) ? null : aktivitetModel.AdditionalNotes.Trim();
    }

    private static DateOnly? ParseDate(string? value)
    {
        return DateOnly.TryParseExact(value, SupportedDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)
            ? parsedDate
            : null;
    }

    private static TimeOnly? ParseTime(string? value)
    {
        return TimeOnly.TryParseExact(value, SupportedTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedTime)
            ? parsedTime
            : null;
    }
}

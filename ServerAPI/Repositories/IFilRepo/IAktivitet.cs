using Core.Model;

namespace ServerAPI.Repositories;

// Interface som definerer de operationer, et aktivitet-repository skal understøtte.
public interface IAktivitet
{
    // Henter alle aktiviteter.
    Task<List<Aktivitet>> GetAllAsync();
    // Henter en enkelt aktivitet ud fra dens id.
    Task<Aktivitet?> GetByIdAsync(string id);
    // Opretter en ny aktivitet.
    Task CreateAsync(Aktivitet aktivitet);
    // Opdaterer en eksisterende aktivitet ud fra id.
    Task UpdateAsync(string id, Aktivitet aktivitet);
    // Sletter en aktivitet ud fra id.
    Task DeleteAsync(string id);
}

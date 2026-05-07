using Core.Model;

namespace ServerAPI.Repositories;

public interface IAktivitetRepository
{
    Task<List<Aktivitet>> GetAllAsync();
    Task<Aktivitet?> GetByIdAsync(string id);
    Task CreateAsync(Aktivitet aktivitet);
    Task UpdateAsync(string id, Aktivitet aktivitet);
    Task DeleteAsync(string id);
}
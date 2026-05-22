using Core.Model;

namespace ServerAPI.Repositories;

public interface IAktivitetRepository
{
    Task<List<AktivitetModel>> GetAllAsync();
    Task<AktivitetModel?> GetByIdAsync(string id);
    Task CreateAsync(AktivitetModel aktivitetModel);
    Task UpdateAsync(string id, AktivitetModel aktivitetModel);
    Task DeleteAsync(string id);
}
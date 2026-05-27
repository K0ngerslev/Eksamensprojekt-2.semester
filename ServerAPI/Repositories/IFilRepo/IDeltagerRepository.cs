using Core.Model;

namespace ServerAPI.Repositories;

public interface IDeltagerRepository
{
    Task<List<DeltagerSvar>> GetByAktivitetIdAsync(string aktivitetId);
    Task UpsertAsync(DeltagerSvar deltagerSvar); // indsæt eller opdater
}

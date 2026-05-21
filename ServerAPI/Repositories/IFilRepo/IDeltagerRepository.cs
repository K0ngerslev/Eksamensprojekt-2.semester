using Core.Model;

namespace ServerAPI.Repositories;

public interface IDeltagerRepository
{
    Task<List<Deltager>> GetByAktivitetIdAsync(string aktivitetId);
    Task UpsertAsync(Deltager deltager); // indsæt eller opdater
}

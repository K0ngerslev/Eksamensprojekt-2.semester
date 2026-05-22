using Core.Model;

namespace ServerAPI.Repositories;

public interface IParticipationRepository
{
    Task<List<Participation>> GetByAktivitetIdAsync(string aktivitetId);
    Task UpsertAsync(Participation participation); // indsæt eller opdater
}

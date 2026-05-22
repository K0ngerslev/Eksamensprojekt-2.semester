using Core.Model;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class ParticipationRepository : IParticipationRepository
{
    private readonly IMongoCollection<Participation> _col;

    public ParticipationRepository()
    {
        var client = new MongoClient("mongodb+srv://eaaa25mo:1234@cluster0.w8idbf2.mongodb.net/");
        var db = client.GetDatabase("Eksamensprojekt");
        _col = db.GetCollection<Participation>("Participations");
    }

    public async Task<List<Participation>> GetByAktivitetIdAsync(string aktivitetId) =>
        await _col.Find(p => p.AktivitetId == aktivitetId).ToListAsync();

    public async Task UpsertAsync(Participation participation)
    {
        // En bruger må højst have ét svar pr. aktivitet.
        var filter = Builders<Participation>.Filter.Where(p =>
            p.AktivitetId == participation.AktivitetId &&
            p.UserName == participation.UserName);

        var existing = await _col.Find(filter).FirstOrDefaultAsync();
        if (existing is null)
            await _col.InsertOneAsync(participation);
        else
        {
            // Genbruger det gamle Mongo-id, så ReplaceOne opdaterer samme dokument.
            participation.Id = existing.Id;
            await _col.ReplaceOneAsync(filter, participation);
        }
    }
}

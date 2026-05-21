using Core.Model;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class DeltagerRepository : IDeltagerRepository
{
    private readonly IMongoCollection<Deltager> _col;

    public DeltagerRepository()
    {
        var client = new MongoClient("mongodb+srv://eaaa25mo:1234@cluster0.w8idbf2.mongodb.net/");
        var db = client.GetDatabase("Eksamensprojekt");
        _col = db.GetCollection<Deltager>("Deltagere");
    }

    public async Task<List<Deltager>> GetByAktivitetIdAsync(string aktivitetId) =>
        await _col.Find(p => p.AktivitetId == aktivitetId).ToListAsync();

    public async Task UpsertAsync(Deltager deltager)
    {
        // En bruger må højst have ét svar pr. aktivitet.
        var filter = Builders<Deltager>.Filter.Where(p =>
            p.AktivitetId == deltager.AktivitetId &&
            p.UserName == deltager.UserName);

        var existing = await _col.Find(filter).FirstOrDefaultAsync();
        if (existing is null)
            await _col.InsertOneAsync(deltager);
        else
        {
            // Genbruger det gamle Mongo-id, så ReplaceOne opdaterer samme dokument.
            deltager.Id = existing.Id;
            await _col.ReplaceOneAsync(filter, deltager);
        }
    }
}

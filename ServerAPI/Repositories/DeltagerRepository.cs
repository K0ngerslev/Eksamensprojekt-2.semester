using Core.Model;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class DeltagerRepository : IDeltagerRepository
{
    private readonly IMongoCollection<DeltagerSvar> col;

    public DeltagerRepository()
    {
        var client = new MongoClient("mongodb+srv://eaaa25mo:1234@cluster0.w8idbf2.mongodb.net/");
        var db = client.GetDatabase("Eksamensprojekt");
        col = db.GetCollection<DeltagerSvar>("Deltagere");
    }

    public async Task<List<DeltagerSvar>> GetByAktivitetIdAsync(string aktivitetId) =>
        await col.Find(p => p.AktivitetId == aktivitetId).ToListAsync();

    public async Task UpsertAsync(DeltagerSvar deltagerSvar)
    {
       //en bruger må højst have ét svar pr. aktivitet
        var filter = Builders<DeltagerSvar>.Filter.Where(p =>
            p.AktivitetId == deltagerSvar.AktivitetId &&
            p.UserName == deltagerSvar.UserName);
        var options = new ReplaceOptions { IsUpsert = true };
        await col.ReplaceOneAsync(filter, deltagerSvar, options);
    }
}

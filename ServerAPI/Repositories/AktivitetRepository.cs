using Core.Model;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

public class AktivitetRepository : IAktivitet
{
    private const string CollectionName = "Aktiviteter";
    private readonly IMongoCollection<Aktivitet> _aktiviteter;

    public AktivitetRepository(IMongoDatabase database)
    {
        _aktiviteter = database.GetCollection<Aktivitet>(CollectionName);
    }

    public async Task<List<Aktivitet>> GetAllAsync()
    {
        return await _aktiviteter.Find(_ => true).ToListAsync();
    }

    public async Task<Aktivitet?> GetByIdAsync(string id)
    {
        return await _aktiviteter.Find(aktivitet => aktivitet.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Aktivitet aktivitet)
    {
        await _aktiviteter.InsertOneAsync(aktivitet);
    }

    public async Task UpdateAsync(string id, Aktivitet aktivitet)
    {
        aktivitet.Id = id;
        await _aktiviteter.ReplaceOneAsync(existingAktivitet => existingAktivitet.Id == id, aktivitet);
    }

    public async Task DeleteAsync(string id)
    {
        await _aktiviteter.DeleteOneAsync(aktivitet => aktivitet.Id == id);
    }
}

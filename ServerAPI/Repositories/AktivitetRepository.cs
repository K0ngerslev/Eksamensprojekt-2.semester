using Core.Model;
using MongoDB.Driver;

namespace ServerAPI.Repositories;

// Repository som håndterer læsning og skrivning af aktiviteter i MongoDB.
public class AktivitetRepository : IAktivitetRepository
{
    // Navnet på den collection i databasen, hvor aktiviteter gemmes.
    private const string CollectionName = "Aktiviteter";
    // Reference til MongoDB-collectionen med aktiviteter.
    private readonly IMongoCollection<Aktivitet> _aktiviteter;

    // Constructor henter den rigtige collection fra databasen.
    public AktivitetRepository()
    {
        // var client = new MongoClient("mongodb+srv://kongersleva_db_user:ctxdw7xMeDXa6BXQ@annoncer.calyub8.mongodb.net/");
        var client = new MongoClient("mongodb+srv://eaaa25mo:1234@cluster0.w8idbf2.mongodb.net/");
        var database = client.GetDatabase("Eksamensprojekt");
        _aktiviteter = database.GetCollection<Aktivitet>("Aktiviteter");
    }

    // Returnerer alle aktiviteter fra collectionen.
    public async Task<List<Aktivitet>> GetAllAsync()
    {
        return await _aktiviteter.Find(_ => true).ToListAsync();
    }

    // Finder en enkelt aktivitet ved at sammenligne på id.
    public async Task<Aktivitet?> GetByIdAsync(string id)
    {
        return await _aktiviteter.Find(aktivitet => aktivitet.Id == id).FirstOrDefaultAsync();
    }

    // Indsætter en ny aktivitet i databasen.
    public async Task CreateAsync(Aktivitet aktivitet)
    {
        await _aktiviteter.InsertOneAsync(aktivitet);
    }

    // Opdaterer en eksisterende aktivitet ved at erstatte dokumentet med samme id.
    public async Task UpdateAsync(string id, Aktivitet aktivitet)
    {
        // Sørger for at objektets id matcher det id, der blev sendt ind.
        aktivitet.Id = id;
        await _aktiviteter.ReplaceOneAsync(existingAktivitet => existingAktivitet.Id == id, aktivitet);
    }

    // Sletter aktiviteten med det angivne id.
    public async Task DeleteAsync(string id)
    {
        await _aktiviteter.DeleteOneAsync(aktivitet => aktivitet.Id == id);
    }
}

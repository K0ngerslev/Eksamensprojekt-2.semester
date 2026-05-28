using MongoDB.Driver;
using Core.Model;

namespace ServerAPI.Repositories;

public class TeamchatRepository : ITeamchatRepository
{
    private readonly IMongoCollection<TeamChatModel> _chatMessages;

    public TeamchatRepository()
    {

        var client = new MongoClient("mongodb+srv://eaaa25mo:1234@cluster0.w8idbf2.mongodb.net/");

        var database = client.GetDatabase("Eksamensprojekt");

        _chatMessages = database.GetCollection<TeamChatModel>("ChatMessages");
    }

    public async Task<List<TeamChatModel>> GetMessagesAsync()
    {
        return await _chatMessages.Find(_ => true)
            .SortBy(m => m.Time)
            .ToListAsync();
    }

    public async Task AddMessageAsync(string user, string text)
    {
        var message = new TeamChatModel
        {
            User = user,
            Text = text,
            Time = DateTime.UtcNow
        };

        await _chatMessages.InsertOneAsync(message);
    }
}
    
using MongoDB.Driver;
using MongoDB.Bson;

namespace ServerAPI.Chat;

public class ChatRepository
{
    private readonly IMongoCollection<ChatMessage> _chatMessages;

    public ChatRepository()
    {
       
        var client = new MongoClient("mongodb+srv://eaaa25mo:1234@cluster0.w8idbf2.mongodb.net/");

        var database = client.GetDatabase("Eksamensprojekt");           

        _chatMessages = database.GetCollection<ChatMessage>("ChatMessages");  
    }

    public async Task<List<ChatMessage>> GetMessagesAsync()
    {
        return await _chatMessages.Find(_ => true)
            .SortBy(m => m.Time)
            .ToListAsync();
    }

    public async Task AddMessageAsync(string user, string text)
    {
        var message = new ChatMessage
        {
            User = user,
            Text = text,
            Time = DateTime.UtcNow
        };

        await _chatMessages.InsertOneAsync(message);
    }

    // Model
    public class ChatMessage
    {
        public ObjectId Id { get; set; }
        public string User { get; set; } = "";
        public string Text { get; set; } = "";
        public DateTime Time { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Model;

public class TeamChatModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? Id { get; set; }

    public string User { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime Time { get; set; }
}
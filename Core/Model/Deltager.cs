using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Model;

public class Deltager
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? Id { get; set; }

    public string? AktivitetId { get; set; }
    public string? UserName { get; set; }
    public bool IsAttending { get; set; }
}
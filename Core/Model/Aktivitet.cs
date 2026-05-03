using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Model;

public class Aktivitet
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? Id { get; set; }

    public string? ActivityType { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public string? FieldOrLocation { get; set; }

    public string? ChangingRoom { get; set; }

    public string? AdditionalNotes { get; set; }
}

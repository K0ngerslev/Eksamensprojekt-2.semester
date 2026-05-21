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

    // Gemmes som "HH:mm" string i MongoDB for at undgå serialiseringsproblemer med TimeOnly
    [BsonElement("StartTime")]
    public string? StartTimeString { get; set; }

    [BsonElement("EndTime")]
    public string? EndTimeString { get; set; }

    // Hjælpeegenskaber til resten af koden — bruges ikke af MongoDB
    [BsonIgnore]
    public TimeOnly? StartTime
    {
        get => TimeOnly.TryParse(StartTimeString, out var t) ? t : null;
        set => StartTimeString = value?.ToString("HH:mm");
    }

    [BsonIgnore]
    public TimeOnly? EndTime
    {
        get => TimeOnly.TryParse(EndTimeString, out var t) ? t : null;
        set => EndTimeString = value?.ToString("HH:mm");
    }

    public string? FieldOrLocation { get; set; }

    [BsonIgnoreIfNull]
    public string? ChangingRoom { get; set; }

    [BsonIgnoreIfNull]
    public string? AdditionalNotes { get; set; }

    public object? Name { get; set; }
    public object? Description { get; set; }
    public object? PublishedDate { get; set; }

    public static string? CreatedBy { get; set; }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Model;

// Modelklasse som repræsenterer en aktivitet i systemet.
public class Aktivitet
{
    // MongoDB bruger dette felt som dokumentets unikke id.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? Id { get; set; }

    // Beskriver typen af aktivitet, f.eks. træning eller kamp.
    public string? ActivityType { get; set; }

    // Angiver hvilken dato aktiviteten foregår på.
    public DateOnly? Date { get; set; }

    // Angiver hvilket tidspunkt aktiviteten starter.
    [BsonElement("Time")]
    public TimeOnly? StartTime { get; set; }

    // Angiver hvilket tidspunkt aktiviteten slutter.
    [BsonIgnoreIfNull]
    public TimeOnly? EndTime { get; set; }

    // Indeholder information om bane eller lokation.
    public string? FieldOrLocation { get; set; }

    // Indeholder eventuelt omklædningsrum for aktiviteten.
    public string? ChangingRoom { get; set; }

    // Indeholder ekstra noter til aktiviteten.
    public string? AdditionalNotes { get; set; }
    public object? Description { get; set; }
}

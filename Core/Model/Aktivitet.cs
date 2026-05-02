using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Model;

public class Aktivitet
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfNull]
    public string? Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string? ActivityType { get; set; }

    [Required]
    public DateOnly? Date { get; set; }

    [Required]
    public TimeOnly? Time { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string? FieldOrLocation { get; set; }

    [StringLength(100)]
    public string? ChangingRoom { get; set; }

    [StringLength(500)]
    public string? AdditionalNotes { get; set; }
}
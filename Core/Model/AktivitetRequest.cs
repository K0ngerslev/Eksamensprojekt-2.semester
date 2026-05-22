namespace Core.Model;

public class AktivitetRequest
{
    public string? ActivityType { get; set; }
    public string? Date { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? FieldOrLocation { get; set; }
    public string? ChangingRoom { get; set; }
    public string? AdditionalNotes { get; set; }
}
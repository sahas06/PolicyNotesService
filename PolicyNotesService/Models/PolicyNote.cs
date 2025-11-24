namespace PolicyNotesService.Models;

public class PolicyNote
{
    public int Id { get; set; }
    public string PolicyNumber { get; set; } = null!;
    public string Note { get; set; } = null!;
}

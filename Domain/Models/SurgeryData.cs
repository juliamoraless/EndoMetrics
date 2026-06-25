using Domain.Models;

public class SurgeryData
{
    public string Id { get; set; }
    public string PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public int InfertilityYears { get; set; }
    public bool PreviousPregnancy { get; set; }
    public SurgicalFindings SurgicalFindings { get; set; }
    public AfsScore AfsScore { get; set; }
}

public record SurgicalFindings(int FimbriaScore, int OvaryScore);
public record AfsScore(int EndometriosisScore, int TotalScore);
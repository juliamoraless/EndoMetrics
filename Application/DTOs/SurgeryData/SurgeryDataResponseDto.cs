namespace Application.DTOs.SurgeryData;

public class SurgeryDataResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public int InfertilityYears { get; set; }
    public bool PreviousPregnancy { get; set; }
    public int FimbriaScore { get; set; }
    public int OvaryScore { get; set; }
    public int EndometriosisScore { get; set; }
    public int TotalScore { get; set; }
}
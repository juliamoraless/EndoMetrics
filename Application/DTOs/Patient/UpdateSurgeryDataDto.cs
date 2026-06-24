namespace Application.DTOs.Patient;

public class UpdateSurgeryDataDto
{
    public int InfertilityYears { get; set; }
    public bool PreviousPregnancy { get; set; }
    public int FimbriaScore { get; set; }
    public int OvaryScore { get; set; }
    public int EndometriosisScore { get; set; }
    public int TotalScore { get; set; }
}
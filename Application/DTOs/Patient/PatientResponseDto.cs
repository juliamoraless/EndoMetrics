namespace Application.DTOs.Patient;

public class PatientResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;
    public string MedicalRecordNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool HasSurgeryData { get; set; } 
}
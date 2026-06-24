namespace Application.DTOs.Patient;

public class UpdatePatientDto
{
    public string BirthDate { get; set; } = string.Empty;
    public string MedicalRecordNumber { get; set; } = string.Empty;
    public UpdateSurgeryDataDto? SurgeryData { get; set; }
}
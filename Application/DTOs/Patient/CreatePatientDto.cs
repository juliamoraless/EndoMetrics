using Application.DTOs.SurgeryData;

namespace Application.DTOs.Patient;

public class CreatePatientDto
{
    public string BirthDate { get; set; } = string.Empty;
    public string MedicalRecordNumber { get; set; } = string.Empty;
    public CreateSurgeryDataDto? SurgeryData { get; set; } 
}
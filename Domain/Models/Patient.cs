namespace Domain.Models;

public class Patient
{
    public string Id { get; set; }
    public string BirthDate { get; set; } = string.Empty;
    public string MedicalRecordNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public SurgeryData? SurgeryData { get; set; }
}
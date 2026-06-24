using Application.DTOs.Patient;

namespace Application.Services;

public interface IPatientService
{
    Task<PatientResponseDto> CreatePatientAsync(CreatePatientDto dto);
    Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();
}
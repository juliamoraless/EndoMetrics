
using Application.DTOs.Patient;

namespace Application.Repositories;

public interface IPatientService
{
    Task<PatientResponseDto> CreatePatientAsync(CreatePatientDto dto);
    Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();
    Task UpdatePatientAsync(string id, UpdatePatientDto dto);
    Task<bool> DeletePatientAsync(string id);
}
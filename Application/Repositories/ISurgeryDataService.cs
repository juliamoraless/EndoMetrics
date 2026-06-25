using Application.DTOs.SurgeryData;

namespace Application.Repositories;

public interface ISurgeryDataService
{
    Task<SurgeryDataResponseDto> CreateAsync(CreateSurgeryDataDto dto);
    Task<IEnumerable<SurgeryDataResponseDto>> GetAllAsync();
    Task<SurgeryDataResponseDto?> GetByIdAsync(string id);
    Task<SurgeryDataResponseDto?> GetByPatientIdAsync(string patientId);
    Task UpdateAsync(string id, UpdateSurgeryDataDto dto);
    Task<bool> DeleteAsync(string id);
}
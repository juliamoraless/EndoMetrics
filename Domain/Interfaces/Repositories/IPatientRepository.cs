using Domain.Models;

namespace Domain.Interfaces;

public interface IPatientRepository
{
    Task<Patient> AddAsync(Patient patient);
    
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(string id);
    Task<Patient?> GetByMedicalRecordNumberAsync(string medicalRecordNumber);
    
    Task UpdateAsync(Patient patient);
    
    Task<bool> DeleteAsync(string id);
}
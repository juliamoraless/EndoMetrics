namespace Domain.Interfaces.Repositories;

public interface ISurgeryDataRepository
{
    // CREATE
    Task<SurgeryData> AddAsync(SurgeryData surgeryData);
    
    // READ
    Task<IEnumerable<SurgeryData>> GetAllAsync();
    Task<SurgeryData?> GetByIdAsync(string id);
    Task<SurgeryData?> GetByPatientIdAsync(string patientId); // Método extra muito útil
    
    // UPDATE
    Task UpdateAsync(SurgeryData surgeryData);
    
    // DELETE
    Task<bool> DeleteAsync(string id);
}
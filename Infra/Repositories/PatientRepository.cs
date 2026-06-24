using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly EndoMetricsContext _context;

    public PatientRepository(EndoMetricsContext context)
    {
        _context = context;
    }

    public async Task<Patient> AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
        
        return patient;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Include(p => p.SurgeryData) 
            .AsNoTracking()              
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(string id)
    {
        return await _context.Patients
            .Include(p => p.SurgeryData)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Patient?> GetByMedicalRecordNumberAsync(string medicalRecordNumber)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.MedicalRecordNumber == medicalRecordNumber);
    }

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var patient = await _context.Patients.FindAsync(id);
        
        if (patient == null) 
            return false;

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        
        return true;
    }
}
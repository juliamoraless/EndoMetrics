using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class SurgeryDataRepository : ISurgeryDataRepository
{
    private readonly EndoMetricsContext _context;

    public SurgeryDataRepository(EndoMetricsContext context)
    {
        _context = context;
    }

    public async Task<SurgeryData> AddAsync(SurgeryData surgeryData)
    {
        await _context.SurgeryDatas.AddAsync(surgeryData);
        await _context.SaveChangesAsync();
        
        return surgeryData;
    }

    public async Task<IEnumerable<SurgeryData>> GetAllAsync()
    {
        return await _context.SurgeryDatas
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<SurgeryData?> GetByIdAsync(string id)
    {
        return await _context.SurgeryDatas
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<SurgeryData?> GetByPatientIdAsync(string patientId)
    {
        return await _context.SurgeryDatas
            .FirstOrDefaultAsync(s => s.PatientId == patientId);
    }

    public async Task UpdateAsync(SurgeryData surgeryData)
    {
        _context.SurgeryDatas.Update(surgeryData);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var surgeryData = await _context.SurgeryDatas.FindAsync(id);
        
        if (surgeryData == null) 
            return false;

        _context.SurgeryDatas.Remove(surgeryData);
        await _context.SaveChangesAsync();
        
        return true;
    }
}
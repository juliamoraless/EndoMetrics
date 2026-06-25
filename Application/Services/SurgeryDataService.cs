using Application.DTOs.SurgeryData;
using Application.Repositories;
using Domain.Interfaces.Repositories;

namespace Application.Services;

public class SurgeryDataService : ISurgeryDataService
{
    private readonly ISurgeryDataRepository _surgeryRepository;
    private readonly IPatientRepository _patientRepository; 
    
    public SurgeryDataService(ISurgeryDataRepository surgeryRepository, IPatientRepository patientRepository)
    {
        _surgeryRepository = surgeryRepository;
        _patientRepository = patientRepository;
    }

    public async Task<SurgeryDataResponseDto> CreateAsync(CreateSurgeryDataDto dto)
    {
        var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
        if (patient == null)
            throw new Exception("Paciente não encontrada para vincular estes dados cirúrgicos.");

        var existingSurgery = await _surgeryRepository.GetByPatientIdAsync(dto.PatientId);
        if (existingSurgery != null)
            throw new Exception("Esta paciente já possui dados cirúrgicos cadastrados. Use o método de atualização.");

        ValidateEfiScores(dto.FimbriaScore, dto.OvaryScore, dto.EndometriosisScore, dto.TotalScore);

        var surgeryData = new SurgeryData
        {
            Id = Guid.NewGuid().ToString(),
            PatientId = dto.PatientId,
            InfertilityYears = dto.InfertilityYears,
            PreviousPregnancy = dto.PreviousPregnancy,
            SurgicalFindings = new SurgicalFindings(dto.FimbriaScore, dto.OvaryScore),
            AfsScore = new AfsScore(dto.EndometriosisScore, dto.TotalScore)
        };

        var result = await _surgeryRepository.AddAsync(surgeryData);

        return MapToResponseDto(result);
    }

    public async Task<IEnumerable<SurgeryDataResponseDto>> GetAllAsync()
    {
        var surgeries = await _surgeryRepository.GetAllAsync();
        return surgeries.Select(MapToResponseDto);
    }

    public async Task<SurgeryDataResponseDto?> GetByIdAsync(string id)
    {
        var surgery = await _surgeryRepository.GetByIdAsync(id);
        return surgery == null ? null : MapToResponseDto(surgery);
    }

    public async Task<SurgeryDataResponseDto?> GetByPatientIdAsync(string patientId)
    {
        var surgery = await _surgeryRepository.GetByPatientIdAsync(patientId);
        return surgery == null ? null : MapToResponseDto(surgery);
    }

    public async Task UpdateAsync(string id, UpdateSurgeryDataDto dto)
    {
        var surgery = await _surgeryRepository.GetByIdAsync(id);
        if (surgery == null)
            throw new Exception("Dados cirúrgicos não encontrados.");

        ValidateEfiScores(dto.FimbriaScore, dto.OvaryScore, dto.EndometriosisScore, dto.TotalScore);

        surgery.InfertilityYears = dto.InfertilityYears;
        surgery.PreviousPregnancy = dto.PreviousPregnancy;
        surgery.SurgicalFindings = new SurgicalFindings(dto.FimbriaScore, dto.OvaryScore);
        surgery.AfsScore = new AfsScore(dto.EndometriosisScore, dto.TotalScore);

        await _surgeryRepository.UpdateAsync(surgery);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _surgeryRepository.DeleteAsync(id);
    }

    private void ValidateEfiScores(int fimbria, int ovary, int endometriosis, int total)
    {
        if (fimbria < 0 || fimbria > 4)
            throw new Exception("O score da fimbria deve ser entre 0 e 4.");

        if (ovary < 0 || ovary > 4)
            throw new Exception("O score do ovário deve ser entre 0 e 4.");

        if (endometriosis < 1 || endometriosis > 71)
            throw new Exception("O score de endometriose AFS deve ser entre 1 e 71.");

        if (total < 1 || total > 150)
            throw new Exception("O score total AFS deve ser entre 1 e 150.");
    }

    private SurgeryDataResponseDto MapToResponseDto(SurgeryData s)
    {
        return new SurgeryDataResponseDto
        {
            Id = s.Id,
            PatientId = s.PatientId,
            InfertilityYears = s.InfertilityYears,
            PreviousPregnancy = s.PreviousPregnancy,
            FimbriaScore = s.SurgicalFindings.FimbriaScore,
            OvaryScore = s.SurgicalFindings.OvaryScore,
            EndometriosisScore = s.AfsScore.EndometriosisScore,
            TotalScore = s.AfsScore.TotalScore
        };
    }
}
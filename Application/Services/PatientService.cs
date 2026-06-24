using Application.DTOs;
using Application.DTOs.Patient;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<PatientResponseDto> CreatePatientAsync(CreatePatientDto dto)
    {
        // Regra de Negócio 1: O Prontuário não pode se repetir
        var existingPatient = await _repository.GetByMedicalRecordNumberAsync(dto.MedicalRecordNumber);
        if (existingPatient != null)
        {
            throw new Exception("Já existe uma paciente cadastrada com este número de prontuário.");
        }

        // Regra de Negócio 2: Validação da Data de Nascimento
        if (!DateTime.TryParse(dto.BirthDate, out DateTime parsedDate))
        {
            throw new Exception("Formato de data de nascimento inválido.");
        }
        if (parsedDate > DateTime.UtcNow)
        {
            throw new Exception("A data de nascimento não pode estar no futuro.");
        }

        // Mapeamento Manual (Você também pode usar bibliotecas como AutoMapper para isso)
        var newPatient = new Patient
        {
            Id = Guid.NewGuid().ToString(), // Gerando um ID único
            BirthDate = dto.BirthDate,
            MedicalRecordNumber = dto.MedicalRecordNumber,
            CreatedAt = DateTime.UtcNow
        };

        if (dto.SurgeryData != null)
        {
            newPatient.SurgeryData = new SurgeryData
            {
                Id = Guid.NewGuid().ToString(),
                PatientId = newPatient.Id,
                InfertilityYears = dto.SurgeryData.InfertilityYears,
                PreviousPregnancy = dto.SurgeryData.PreviousPregnancy,
                SurgicalFindings = new SurgicalFindings(dto.SurgeryData.FimbriaScore, dto.SurgeryData.OvaryScore),
                AfsScore = new AfsScore(dto.SurgeryData.EndometriosisScore, dto.SurgeryData.TotalScore)
            };
        }

        // Salva no banco via repositório
        var savedPatient = await _repository.AddAsync(newPatient);

        // Mapeia para o DTO de saída e retorna
        return new PatientResponseDto
        {
            Id = savedPatient.Id,
            BirthDate = savedPatient.BirthDate,
            MedicalRecordNumber = savedPatient.MedicalRecordNumber,
            CreatedAt = savedPatient.CreatedAt,
            HasSurgeryData = savedPatient.SurgeryData != null
        };
    }

    public async Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync()
    {
        var patients = await _repository.GetAllAsync();

        return patients.Select(p => new PatientResponseDto
        {
            Id = p.Id,
            BirthDate = p.BirthDate,
            MedicalRecordNumber = p.MedicalRecordNumber,
            CreatedAt = p.CreatedAt,
            HasSurgeryData = p.SurgeryData != null
        });
    }
}
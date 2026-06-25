using Application.DTOs;
using Application.DTOs.Patient;
using Application.Repositories;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
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
    public async Task UpdatePatientAsync(string id, UpdatePatientDto dto)
{
    // 1. Busca a paciente existente (já trazendo os dados cirúrgicos atuais)
    var patient = await _repository.GetByIdAsync(id);
    if (patient == null)
    {
        throw new Exception("Paciente não encontrada.");
    }

    // 2. Valida se o novo prontuário já pertence a outra paciente
    if (patient.MedicalRecordNumber != dto.MedicalRecordNumber)
    {
        var duplicatePatient = await _repository.GetByMedicalRecordNumberAsync(dto.MedicalRecordNumber);
        if (duplicatePatient != null)
        {
            throw new Exception("Já existe outra paciente cadastrada com este número de prontuário.");
        }
    }

    // 3. Valida a nova data de nascimento
    if (!DateTime.TryParse(dto.BirthDate, out DateTime parsedDate))
    {
        throw new Exception("Formato de data de nascimento inválido.");
    }
    if (parsedDate > DateTime.UtcNow)
    {
        throw new Exception("A data de nascimento não pode estar no futuro.");
    }

    // 4. Atualiza os dados da entidade principal
    patient.BirthDate = dto.BirthDate;
    patient.MedicalRecordNumber = dto.MedicalRecordNumber;

    // 5. Atualiza ou adiciona os dados cirúrgicos
    if (dto.SurgeryData != null)
    {
        if (patient.SurgeryData == null)
        {
            // Se ela não tinha dados cirúrgicos antes, cria um novo
            patient.SurgeryData = new SurgeryData
            {
                Id = Guid.NewGuid().ToString(),
                PatientId = patient.Id,
                InfertilityYears = dto.SurgeryData.InfertilityYears,
                PreviousPregnancy = dto.SurgeryData.PreviousPregnancy,
                SurgicalFindings = new SurgicalFindings(dto.SurgeryData.FimbriaScore, dto.SurgeryData.OvaryScore),
                AfsScore = new AfsScore(dto.SurgeryData.EndometriosisScore, dto.SurgeryData.TotalScore)
            };
        }
        else
        {
            // Se já tinha, substitui os valores (os records são imutáveis, então criamos uma nova instância deles)
            patient.SurgeryData.InfertilityYears = dto.SurgeryData.InfertilityYears;
            patient.SurgeryData.PreviousPregnancy = dto.SurgeryData.PreviousPregnancy;
            patient.SurgeryData.SurgicalFindings = new SurgicalFindings(dto.SurgeryData.FimbriaScore, dto.SurgeryData.OvaryScore);
            patient.SurgeryData.AfsScore = new AfsScore(dto.SurgeryData.EndometriosisScore, dto.SurgeryData.TotalScore);
        }
    }
    else
    {
        // Se na atualização o objeto SurgeryData veio nulo, significa que queremos remover os dados cirúrgicos dela
        patient.SurgeryData = null;
    }

    // 6. Envia para o repositório salvar
    await _repository.UpdateAsync(patient);
}

public async Task<bool> DeletePatientAsync(string id)
{
    // O Cascade que configuramos no OnModelCreating garante que 
    // se o repositório deletar a Patient, o banco deleta o SurgeryData dela sozinho.
    return await _repository.DeleteAsync(id);
}
    
    
}
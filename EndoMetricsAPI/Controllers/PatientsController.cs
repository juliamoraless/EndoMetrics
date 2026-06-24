using Application.DTOs;
using Application.DTOs.Patient;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EndoMetricsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
    {
        try
        {
            var result = await _patientService.CreatePatientAsync(dto);
            
            // Retorna o status 201 Created se der tudo certo
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            // Em caso de erro de validação (como prontuário repetido), devolve 400 Bad Request
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await _patientService.GetAllPatientsAsync();
        return Ok(patients); // Retorna 200 OK com a lista
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdatePatientDto dto)
    {
        try
        {
            await _patientService.UpdatePatientAsync(id, dto);
            return NoContent(); // Retorna 204 NoContent padrão para atualizações bem-sucedidas
        }
        catch (Exception ex)
        {
            if (ex.Message == "Paciente não encontrada.")
                return NotFound(new { Error = ex.Message });

            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _patientService.DeletePatientAsync(id);
    
        if (!deleted)
        {
            return NotFound(new { Error = "Paciente não encontrada." });
        }

        return NoContent(); // Retorna 204 NoContent para deleções bem-sucedidas
    }
}
using Application.DTOs.SurgeryData;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EndoMetricsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurgeryDataController : ControllerBase
{
    private readonly ISurgeryDataService _surgeryService;

    public SurgeryDataController(ISurgeryDataService surgeryService)
    {
        _surgeryService = surgeryService;
    }

    // POST: api/surgerydata
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSurgeryDataDto dto)
    {
        try
        {
            var result = await _surgeryService.CreateAsync(dto);
            
            // Retorna 201 Created e informa a rota para buscar este recurso recém-criado
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            // Trata as validações de negócio (limites do EFI, paciente inexistente, etc)
            return BadRequest(new { Error = ex.Message });
        }
    }

    // GET: api/surgerydata
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var surgeries = await _surgeryService.GetAllAsync();
        return Ok(surgeries);
    }

    // GET: api/surgerydata/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var surgery = await _surgeryService.GetByIdAsync(id);
        
        if (surgery == null)
            return NotFound(new { Error = "Dados cirúrgicos não encontrados." });

        return Ok(surgery);
    }

    // GET: api/surgerydata/patient/{patientId}
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatientId(string patientId)
    {
        var surgery = await _surgeryService.GetByPatientIdAsync(patientId);
        
        if (surgery == null)
            return NotFound(new { Error = "Nenhum dado cirúrgico encontrado para esta paciente." });

        return Ok(surgery);
    }

    // PUT: api/surgerydata/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateSurgeryDataDto dto)
    {
        try
        {
            await _surgeryService.UpdateAsync(id, dto);
            return NoContent(); // 204 No Content - Padrão REST para atualizações com sucesso
        }
        catch (Exception ex)
        {
            if (ex.Message == "Dados cirúrgicos não encontrados.")
                return NotFound(new { Error = ex.Message });

            return BadRequest(new { Error = ex.Message });
        }
    }

    // DELETE: api/surgerydata/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _surgeryService.DeleteAsync(id);
        
        if (!deleted)
            return NotFound(new { Error = "Dados cirúrgicos não encontrados." });

        return NoContent();
    }
}
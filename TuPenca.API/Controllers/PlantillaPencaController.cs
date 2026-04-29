using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.PlantillaPenca;
using TuPenca.Application.Interfaces.Services;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AdministradorPlataforma")]
    public class PlantillaPencaController : ControllerBase
    {
        private readonly IPlantillaPencaService _plantillaService;

        public PlantillaPencaController(IPlantillaPencaService plantillaService)
        {
            _plantillaService = plantillaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var response = await _plantillaService.ObtenerTodasAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var response = await _plantillaService.ObtenerPorIdAsync(id);
            if (response == null) return NotFound("Plantilla no encontrada");
            return Ok(response);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] PlantillaPencaRequestDto dto)
        {
            try
            {
                var response = await _plantillaService.CrearAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                await _plantillaService.EliminarAsync(id);
                return Ok("Plantilla eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TuPenca.Application.DTOs.Penca;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Enums;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AdministradorSitio")]
    public class PencaController : ControllerBase
    {
        private readonly IPencaService _pencaService;

        public PencaController(IPencaService pencaService)
        {
            _pencaService = pencaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var response = await _pencaService.ObtenerTodasAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var response = await _pencaService.ObtenerPorIdAsync(id);
            if (response == null) return NotFound("Penca no encontrada");
            return Ok(response);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] PencaRequestDto dto)
        {
            try
            {
                var sitioIdClaim = User.FindFirst("sitioId")?.Value;
                if (sitioIdClaim == null)
                    return Unauthorized("No se pudo determinar el sitio");

                var sitioId = Guid.Parse(sitioIdClaim);
                var response = await _pencaService.CrearAsync(dto, sitioId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] EstadoPenca nuevoEstado)
        {
            try
            {
                var response = await _pencaService.CambiarEstadoAsync(id, nuevoEstado);
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
                await _pencaService.EliminarAsync(id);
                return Ok("Penca eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
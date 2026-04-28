using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.Sitio;
using TuPenca.Application.Interfaces.Services;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitioController : ControllerBase
    {
        private readonly ISitioService _sitioService;

        public SitioController(ISitioService sitioService)
        {
            _sitioService = sitioService;
        }

        [HttpGet("obtener/todos")]
        [Authorize(Roles = "AdministradorPlataforma")]
        public async Task<IActionResult> ObtenerSitiosAsync()
        {
            try
            {
                var response = await _sitioService.ObtenerSitiosAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obtener/{sitioId}")]
        [Authorize(Roles = "AdministradorSitio,AdministradorPlataforma")] 
        public async Task<IActionResult> ObtenerSitioAsync(Guid sitioId)
        {
            try
            {
                var response = await _sitioService.ObtenerSitioAsync(sitioId);
                if (response == null)
                    return NotFound("Sitio no encontrado");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("crear")]
        [Authorize(Roles = "AdministradorPlataforma")]
        public async Task<IActionResult> CrearSitioAsync([FromBody] SitioRequestDto sitioDto)
        {
            try
            {
                var response = await _sitioService.CrearSitioAsync(sitioDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("actualizar")]
        [Authorize(Roles = "AdministradorPlataforma")]
        public async Task<IActionResult> ActualizarSitioAsync([FromBody] SitioRequestDto sitioDto)
        {
            try
            {
                var response = await _sitioService.ActualizarSitioAsync(sitioDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("eliminar/{sitioId}")]
        [Authorize(Roles = "AdministradorPlataforma")]
        public async Task<IActionResult> EliminarSitioAsync(Guid sitioId)
        {
            try
            {
                var response = await _sitioService.EliminarSitioAsync(sitioId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

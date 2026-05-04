using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.Deporte;
using TuPenca.Application.Interfaces.Services;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AdministradorPlataforma")]
    public class DeporteController : Controller
    {
        private readonly IDeporteService _deporteService;

        public DeporteController(IDeporteService deporteService)
        {
            _deporteService = deporteService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _deporteService.ObtenerTodosAsync();
            return Ok(response);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearVarios([FromBody] DeporteRequestDto dto)
        {
            try
            {
                var response = await _deporteService.CrearVariosAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

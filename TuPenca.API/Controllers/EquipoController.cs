using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.Equipo;
using TuPenca.Application.Interfaces.Services;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly IEquipoService _equipoService;

        public EquipoController(IEquipoService equipoService)
        {
            _equipoService = equipoService;
        }

        [HttpGet]
        [Authorize(Roles = "AdministradorPlataforma")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _equipoService.ObtenerTodosAsync();
            return Ok(response);
        }

        [HttpPost("crear")]
        [Authorize(Roles = "AdministradorPlataforma")]
        public async Task<IActionResult> CrearVarios([FromBody] EquipoRequestDto dto)
        {
            try
            {
                var response = await _equipoService.CrearVariosAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
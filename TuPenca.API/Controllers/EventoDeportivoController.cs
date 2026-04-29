using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.EventoDeportivo;
using TuPenca.Application.DTOs.Partido;
using TuPenca.Application.Interfaces.Services;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AdministradorPlataforma")]
    public class EventoDeportivoController : ControllerBase
    {
        private readonly IEventoDeportivoService _eventoService;

        public EventoDeportivoController(IEventoDeportivoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _eventoService.ObtenerTodosAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var response = await _eventoService.ObtenerPorIdAsync(id);
            if (response == null) return NotFound("Evento deportivo no encontrado");
            return Ok(response);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] EventoDeportivoRequestDto dto)
        {
            try
            {
                var response = await _eventoService.CrearAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("partido/agregar")]
        public async Task<IActionResult> AgregarPartido([FromBody] PartidoRequestDto dto)
        {
            try
            {
                var response = await _eventoService.AgregarPartidoAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
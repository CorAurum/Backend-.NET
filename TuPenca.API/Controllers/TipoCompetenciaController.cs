using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.TipoCompetencia;
using TuPenca.Application.Interfaces.Services;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AdministradorPlataforma")]
    public class TipoCompetenciaController : Controller
    {
        private readonly ITipoCompetenciaService _tipoCompetenciaService;

        public TipoCompetenciaController(ITipoCompetenciaService tipoCompetenciaService)
        {
            _tipoCompetenciaService = tipoCompetenciaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _tipoCompetenciaService.ObtenerTodosAsync();
            return Ok(response);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearVarios([FromBody] TipoCompetenciaRequestDto dto)
        {
            try
            {
                var response = await _tipoCompetenciaService.CrearVariosAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AdministradorPlataforma")]
    public class TipoCompetenciaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoCompetenciaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var tipos = await _unitOfWork.TiposCompetencia.GetAllAsync();
            return Ok(tipos.Select(t => new { t.Id, t.Nombre }));
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] string nombre)
        {
            try
            {
                var tipo = new TipoCompetencia { Nombre = nombre };
                await _unitOfWork.TiposCompetencia.AddAsync(tipo);
                await _unitOfWork.SaveChangesAsync();
                return Ok(new { tipo.Id, tipo.Nombre });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

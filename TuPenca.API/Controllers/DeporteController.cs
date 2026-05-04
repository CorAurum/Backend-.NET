using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AdministradorPlataforma")]
    public class DeporteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeporteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var deportes = await _unitOfWork.Deportes.GetAllAsync();
            return Ok(deportes.Select(d => new { d.Id, d.Nombre }));
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] string nombre)
        {
            try
            {
                var deporte = new Deporte { Nombre = nombre };
                await _unitOfWork.Deportes.AddAsync(deporte);
                await _unitOfWork.SaveChangesAsync();
                return Ok(new { deporte.Id, deporte.Nombre });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

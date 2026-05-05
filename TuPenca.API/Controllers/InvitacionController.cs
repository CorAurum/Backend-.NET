using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TuPenca.Application.DTOs.Invitacion;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Infrastructure.Interfaces.Providers;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitacionController : ControllerBase
    {
        private readonly IInvitacionService _invitacionService;
        private readonly ISitioProvider _sitioProvider;

        public InvitacionController(IInvitacionService invitacionService, ISitioProvider sitioProvider)
        {
            _invitacionService = invitacionService;
            _sitioProvider = sitioProvider;
        }

        [HttpPost("generar")]
        [Authorize(Roles = "UsuarioComun,AdministradorSitio")]
        public async Task<IActionResult> Generar([FromBody] InvitacionRequestDto dto)
        {
            try
            {
                var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var sitioId = _sitioProvider.GetSitioId();
                if (sitioId == null)
                    return Unauthorized("No se pudo determinar el sitio");

                var response = await _invitacionService.GenerarInvitacionAsync(dto, usuarioId, sitioId.Value);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "AdministradorSitio")]
        public async Task<IActionResult> ObtenerInvitaciones()
        {
            try
            {
                var sitioId = _sitioProvider.GetSitioId();
                if (sitioId == null)
                    return Unauthorized("No se pudo determinar el sitio");

                var response = await _invitacionService.ObtenerInvitacionesSitioAsync(sitioId.Value);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
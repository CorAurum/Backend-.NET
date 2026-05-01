using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordGenerator;
using TuPenca.Application.DTOs.Auth;
using TuPenca.Application.DTOs.Sitio;
using TuPenca.Application.DTOs.Usuario;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Infrastructure.Interfaces.Providers;

namespace TuPenca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitioController : ControllerBase
    {
        private readonly ISitioService _sitioService;
        private readonly IAuthService _authService;
        private readonly IUsuarioService _usuarioService;
        private readonly Guid? _sitioId;

        public SitioController(ISitioService sitioService, 
            IAuthService authService,
            IUsuarioService usuarioService,
            ISitioProvider sitioProvider)
        {
            _sitioService = sitioService;
            _authService = authService;
            _usuarioService = usuarioService;
            _sitioId = sitioProvider.GetSitioId();
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

        [HttpGet("obtener/pendientes")]
        [Authorize(Roles = "AdministradorPlataforma")]
        public async Task<IActionResult> ObtenerSitiosPendientesAsync()
        {
            try
            {
                var response = await _sitioService.ObtenerSitiosPendientesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pendiente/solicitar")]
        public async Task<IActionResult> SolicitarSitioAsync([FromBody] SitioPendienteRequestDto solicitarSitioDto)
        {
            try
            {
                var response = await _sitioService.SolicitarSitioAsync(solicitarSitioDto);

                var pwd = new Password(9);
                var password = pwd.Next();

                var userRequest = new RegistroUsuarioRequestDto()
                {
                    Email = solicitarSitioDto.Email,
                    Nombre = solicitarSitioDto.NombreUsuario,
                    Password = password,
                    Rol = Domain.Enums.RolUsuario.AdministradorSitio
                };
                
                await _authService.RegistrarUsuarioAsync(userRequest, _sitioId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pendiente/actualizar")]
        [Authorize(Roles = "AdministradorPlataforma")]
        public async Task<IActionResult> ActualizarSitioPendienteAsync([FromBody] SitioPendienteActualizarRequestDto sitioDto)
        {
            try
            {
                var response = await _sitioService.ActualizarSitioPendienteAsync(sitioDto);

                var usuarios = await _usuarioService.ObtenerUsuariosAsync();
                var usuario = usuarios.Where(u => u.SitioId == sitioDto.Id)?.FirstOrDefault();

                if(usuario != null)
                {
                    var usuarioDto = new UsuarioActualizarEstadoRequestDto()
                    {
                        Id = usuario.Id,
                        Estado = sitioDto.Estado == Domain.Enums.EstadoSitio.Activo ? Domain.Enums.EstadoUsuario.Aprobado : Domain.Enums.EstadoUsuario.Rechazado
                    };

                    await _usuarioService.ActualizarEstadoAsync(usuarioDto);
                }

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

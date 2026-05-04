using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.Auth;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Infrastructure.Interfaces.Providers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly Guid? _sitioId;

    public AuthController(IAuthService authService, ISitioProvider sitioProvider)
    {
        _authService = authService;
        _sitioId = sitioProvider.GetSitioId();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Email y contraseña son requeridos");

        // Primero intenta como AdministradorPlataforma (sin sitioId)
        var responseAdmin = await _authService.LoginAsync(request, null);
        if (responseAdmin != null && responseAdmin.Rol == "AdministradorPlataforma")
            return Ok(responseAdmin);

        // Luego intenta como usuario del sitio
        var response = await _authService.LoginAsync(request, _sitioId);

        if (response == null)
            return Unauthorized("Credenciales incorrectas");

        return Ok(response);
    }

    [HttpPost("registro/usuario")]
    public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroUsuarioRequestDto request)
    {
        try
        {
            var response = await _authService.RegistrarUsuarioAsync(request, _sitioId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("registro/administrador")]
    [Authorize(Roles = "AdministradorPlataforma")]
    public async Task<IActionResult> RegistrarAdmin([FromBody] RegistroAdminRequestDto request)
    {
        try
        {
            var response = await _authService.RegistrarAdminAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // ⚠️ TEMPORAL — eliminar después de crear el primer admin
    [HttpPost("setup/primer-admin")]
    public async Task<IActionResult> CrearPrimerAdmin([FromBody] RegistroAdminRequestDto request)
    {
        try
        {
            var response = await _authService.RegistrarAdminAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
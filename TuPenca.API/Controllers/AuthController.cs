using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.Auth;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Infrastructure.Interfaces.Providers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ISitioProvider _sitioProvider;

    public AuthController(IAuthService authService, ISitioProvider sitioProvider)
    {
        _authService = authService;
        _sitioProvider = sitioProvider;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var sitioId = _sitioProvider.GetSitioId();

        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Email y contraseña son requeridos");

        var response = await _authService.LoginAsync(request, sitioId);

        if (response == null)
            return Unauthorized("Credenciales incorrectas");

        return Ok(response);
    }

    [HttpPost("registro/usuario")]
    public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroUsuarioRequestDto request)
    {
        try
        {
            var sitioId = _sitioProvider.GetSitioId();
            var response = await _authService.RegistrarUsuarioAsync(request, sitioId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("registro/administrador")]
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

}
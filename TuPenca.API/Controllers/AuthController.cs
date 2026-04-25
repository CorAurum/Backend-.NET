using Microsoft.AspNetCore.Mvc;
using TuPenca.Application.DTOs.Auth;
using TuPenca.Application.Interfaces.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Email y contraseña son requeridos");

        var response = await _authService.LoginAsync(request);

        if (response == null)
            return Unauthorized("Credenciales incorrectas");

        return Ok(response);
    }
}
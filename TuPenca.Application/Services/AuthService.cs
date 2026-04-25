using Microsoft.AspNetCore.Identity;
using TuPenca.Application.DTOs.Auth;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasher<string> _hasher = new();

    public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        // 1. If SitioId provided, try Usuario first
        if (request.SitioId.HasValue)
        {
            var usuario = await _unitOfWork.Usuarios
                .GetByEmailAsync(request.Email, request.SitioId.Value);

            if (usuario != null)
            {
                var resultado = _hasher.VerifyHashedPassword(null!, usuario.PasswordHash, request.Password);
                if (resultado == PasswordVerificationResult.Failed)
                    return null;

                var token = _jwtService.GenerarToken(
                    usuario.Id.ToString(), usuario.Email, usuario.Nombre, "Usuario");

                return new LoginResponseDto
                {
                    Token = token,
                    Nombre = usuario.Nombre,
                    Rol = "Usuario",
                    Expira = DateTime.UtcNow.AddHours(8)
                };
            }
        }

        // 2. Fallback: try Administrador (no SitioId needed)
        var admins = await _unitOfWork.Administrador.GetAllAsync();
        var admin = admins.FirstOrDefault(a => a.Email == request.Email);

        if (admin != null)
        {
            var resultado = _hasher.VerifyHashedPassword(null!, admin.PasswordHash, request.Password);
            if (resultado == PasswordVerificationResult.Failed)
                return null;

            var token = _jwtService.GenerarToken(
                admin.Id.ToString(), admin.Email, admin.Email, "Administrador");

            return new LoginResponseDto
            {
                Token = token,
                Nombre = admin.Email,
                Rol = "Administrador",
                Expira = DateTime.UtcNow.AddHours(8)
            };
        }

        return null; // not found anywhere
    }

    public string HashPassword(string password)
        => _hasher.HashPassword(null!, password);

    public bool VerifyPassword(string password, string hash)
        => _hasher.VerifyHashedPassword(null!, hash, password)
           != PasswordVerificationResult.Failed;
}
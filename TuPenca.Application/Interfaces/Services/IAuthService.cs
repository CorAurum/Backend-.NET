using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Application.DTOs.Auth;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, Guid? sitioId);
        Task<RegistroResponseDto> RegistrarUsuarioAsync(RegistroUsuarioRequestDto request, Guid? sitioId);
        Task<RegistroResponseDto> RegistrarAdminAsync(RegistroAdminRequestDto request);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}

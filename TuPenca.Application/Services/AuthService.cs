using Microsoft.AspNetCore.Identity;
using TuPenca.Application.DTOs.Auth;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;
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
        // ¿Viene con SitioId? → es Usuario del sitio (común o admin de sitio)
        if (request.SitioId.HasValue)
        {
            var usuario = await _unitOfWork.Usuarios
                .GetByEmailAsync(request.Email, request.SitioId.Value);

            if (usuario == null) return null;

            var resultado = _hasher.VerifyHashedPassword(
                null!, usuario.PasswordHash, request.Password);

            if (resultado == PasswordVerificationResult.Failed)
                return null;

            // El rol viene del campo Rol de la entidad
            var rolClaim = usuario.Rol == RolUsuario.AdministradorSitio
                ? "AdministradorSitio"
                : "UsuarioComun";

            var token = _jwtService.GenerarToken(
               usuario.Id.ToString(),
               usuario.Email,
               usuario.Nombre,
               rolClaim,
               usuario.SitioId.ToString() // ← agregado
           );

            return new LoginResponseDto
            {
                Token = token,
                Nombre = usuario.Nombre,
                Rol = rolClaim,
                Expira = DateTime.UtcNow.AddHours(8)
            };
        }
        else
        {
            // Sin SitioId → es Administrador de plataforma
            var admins = await _unitOfWork.Administrador.GetAllAsync();
            var admin = admins.FirstOrDefault(a => a.Email == request.Email);

            if (admin == null) return null;

            var resultado = _hasher.VerifyHashedPassword(
                null!, admin.PasswordHash, request.Password);

            if (resultado == PasswordVerificationResult.Failed)
                return null;

            // Login de AdministradorPlataforma — sin SitioId
            var token = _jwtService.GenerarToken(
                admin.Id.ToString(),
                admin.Email,
                admin.Email,
                "AdministradorPlataforma"
            // sin sitioId → queda null
            );


            return new LoginResponseDto
            {
                Token = token,
                Nombre = admin.Email,
                Rol = "AdministradorPlataforma",
                Expira = DateTime.UtcNow.AddHours(8)
            };
        }
    }

    public async Task<RegistroResponseDto> RegistrarUsuarioAsync(RegistroUsuarioRequestDto request)
    {
        var sitio = await _unitOfWork.Sitios.GetByIdAsync(request.SitioId);
        if (sitio == null)
            throw new Exception("Sitio no encontrado");

        if (sitio.TipoRegistro == TipoRegistro.Cerrada)
            throw new Exception("Este sitio no acepta registros");

        var existente = await _unitOfWork.Usuarios
            .GetByEmailAsync(request.Email, request.SitioId);
        if (existente != null)
            throw new Exception("El email ya está en uso en este sitio");

        if (sitio.TipoRegistro == TipoRegistro.Con_Invitacion)
        {
            if (string.IsNullOrEmpty(request.CodigoInvitacion))
                throw new Exception("Este sitio requiere un código de invitación");

            var invitaciones = await _unitOfWork.Invitaciones.GetAllAsync();
            var invitacion = invitaciones.FirstOrDefault(i =>
                i.Codigo == request.CodigoInvitacion &&
                i.EmailInvitado == request.Email &&
                !i.Aceptada);

            if (invitacion == null)
                throw new Exception("Código de invitación inválido o ya utilizado");

            invitacion.Aceptada = true;
            await _unitOfWork.Invitaciones.UpdateAsync(invitacion);
        }

        var estadoInicial = sitio.TipoRegistro == TipoRegistro.Abierta
            ? EstadoUsuario.Aprobado
            : EstadoUsuario.Pendiente;

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            SitioId = request.SitioId,
            TenantId = request.SitioId,
            Estado = estadoInicial,
            Rol = request.Rol, // ← viene del request, puede ser UsuarioComun o AdministradorSitio
            FechaRegistro = DateTime.UtcNow
        };

        await _unitOfWork.Usuarios.AddAsync(usuario);
        await _unitOfWork.SaveChangesAsync();

        var mensaje = estadoInicial == EstadoUsuario.Aprobado
            ? "Registro exitoso"
            : "Registro exitoso, pendiente de aprobación por un administrador";

        return new RegistroResponseDto
        {
            Id = usuario.Id,
            Email = usuario.Email,
            Mensaje = mensaje
        };
    }

    public async Task<RegistroResponseDto> RegistrarAdminAsync(RegistroAdminRequestDto request)
    {
        // Verificar que no exista ya un admin con ese email
        var admins = await _unitOfWork.Administrador.GetAllAsync();
        var existente = admins.FirstOrDefault(a => a.Email == request.Email);
        if (existente != null)
            throw new Exception("El email ya está en uso");

        var admin = new Administrador
        {
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            FechaRegistro = DateTime.UtcNow
            // Sin SitioId
        };

        await _unitOfWork.Administrador.AddAsync(admin);
        await _unitOfWork.SaveChangesAsync();

        return new RegistroResponseDto
        {
            Id = admin.Id,
            Email = admin.Email,
            Mensaje = "Administrador de plataforma registrado exitosamente"
        };
    }

    public string HashPassword(string password)
        => _hasher.HashPassword(null!, password);

    public bool VerifyPassword(string password, string hash)
        => _hasher.VerifyHashedPassword(null!, hash, password)
           != PasswordVerificationResult.Failed;
}
using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Auth
{
    public class RegistroUsuarioRequestDto
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public RolUsuario Rol { get; set; } = RolUsuario.UsuarioComun;
        public string? CodigoInvitacion { get; set; }
    }
}

using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Usuario
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public RolUsuario Rol { get; set; }
        public DateTime FechaRegistro { get; set; }
        public EstadoUsuario Estado { get; set; }
        public Guid SitioId { get; set; }
    }
}

using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Usuario
{
    public class UsuarioActualizarEstadoRequestDto
    {
        public Guid Id { get; set; }
        public EstadoUsuario Estado { get; set; }
    }
}

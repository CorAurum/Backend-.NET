namespace TuPenca.Application.DTOs.Auth
{
    public class AprobacionUsuarioDto
    {
        public Guid UsuarioId { get; set; }
    }

    public class UsuarioPendienteDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Sitio
{
    public class SitioPendienteActualizarRequestDto
    {
        public Guid Id { get; set; }
        public EstadoSitio Estado { get; set; }
    }
}

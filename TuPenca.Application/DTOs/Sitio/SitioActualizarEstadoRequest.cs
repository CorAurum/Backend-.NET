using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Sitio
{
    public class SitioActualizarEstadoRequest
    {
        public Guid Id { get; set; }
        public EstadoSitio Estado { get; set; }
    }
}

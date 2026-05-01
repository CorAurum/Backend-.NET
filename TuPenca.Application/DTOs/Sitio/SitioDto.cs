using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Sitio
{
    public class SitioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string UrlPropia { get; set; } = null!;
        public string ColorPrimario { get; set; } = null!;
        public string ColorSecundario { get; set; } = null!;
        public string? ConfiguracionSitio { get; set; } = null!;
        public TipoRegistro TipoRegistro { get; set; } = TipoRegistro.Abierta;
        public EstadoSitio Estado { get; set; } = EstadoSitio.Pendiente;
    }
}

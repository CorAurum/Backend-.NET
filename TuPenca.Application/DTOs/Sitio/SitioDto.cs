using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Sitio
{
    public class SitioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string UrlPropia { get; set; } = null!;
        public string ColorPrimario { get; set; }
        public string ColorSecundario { get; set; }
        public string? ConfiguracionSitio { get; set; }
        public TipoRegistro TipoRegistro { get; set; }
    }
}

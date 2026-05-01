using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Sitio
{
    public class SitioRequestDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string UrlPropia { get; set; } = null!;
        public string ColorPrimario { get; set; } = null!;
        public string ColorSecundario { get; set; } = null!;
        public string? ConfiguracionSitio { get; set; }
        public TipoRegistro TipoRegistro { get; set; } = TipoRegistro.Abierta;
    }
}

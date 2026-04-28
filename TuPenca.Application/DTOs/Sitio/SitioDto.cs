using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Sitio
{
    public class SitioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string UrlPropia { get; set; } = null!;
        public string EsquemaColores { get; set; }
        public string? ConfiguracionSitio { get; set; }
        public TipoRegistro TipoRegistro { get; set; }
    }
}

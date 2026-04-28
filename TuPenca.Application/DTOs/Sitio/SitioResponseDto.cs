using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Sitio
{
    public class SitioResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
    }
}

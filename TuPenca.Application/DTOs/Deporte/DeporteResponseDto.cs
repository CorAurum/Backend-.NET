using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Deporte
{
    public class DeporteResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}

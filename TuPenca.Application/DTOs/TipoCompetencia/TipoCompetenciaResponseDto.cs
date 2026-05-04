using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.TipoCompetencia
{
    public class TipoCompetenciaResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}

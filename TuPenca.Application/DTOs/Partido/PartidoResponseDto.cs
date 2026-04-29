using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Partido
{
    public class PartidoResponseDto
    {
        public Guid Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Fase { get; set; } = null!;
        public string EquipoLocal { get; set; } = null!;
        public string EquipoVisitante { get; set; } = null!;
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
    }
}
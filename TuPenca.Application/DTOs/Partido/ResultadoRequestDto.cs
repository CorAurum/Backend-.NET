using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Partido
{
    public class ResultadoRequestDto
    {
        public Guid PartidoId { get; set; }
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
    }
}
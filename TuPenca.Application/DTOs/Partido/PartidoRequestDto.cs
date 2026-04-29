using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Partido
{
    public class PartidoRequestDto
    {
        public DateTime Fecha { get; set; }
        public string Fase { get; set; } = null!;
        public Guid EquipoLocalId { get; set; }
        public Guid EquipoVisitanteId { get; set; }
        public Guid EventoDeportivoId { get; set; }
    }
}
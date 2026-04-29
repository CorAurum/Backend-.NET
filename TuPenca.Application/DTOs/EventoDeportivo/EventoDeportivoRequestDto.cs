using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.EventoDeportivo
{
    public class EventoDeportivoRequestDto
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Guid DeporteId { get; set; }
        public Guid TipoCompetenciaId { get; set; }
    }
}
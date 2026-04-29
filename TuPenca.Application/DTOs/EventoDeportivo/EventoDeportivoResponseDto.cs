using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.EventoDeportivo
{
    public class EventoDeportivoResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}

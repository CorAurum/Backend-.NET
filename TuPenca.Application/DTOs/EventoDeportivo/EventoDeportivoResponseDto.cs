using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Application.DTOs.Partido;

namespace TuPenca.Application.DTOs.EventoDeportivo
{
    public class EventoDeportivoResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<PartidoResponseDto>? Partidos { get; set; }
    }
}

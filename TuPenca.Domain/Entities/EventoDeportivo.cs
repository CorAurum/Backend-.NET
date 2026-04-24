using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class EventoDeportivo : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // N EventoDeportivo → 1 Deporte
        public Guid DeporteId { get; set; }
        public Deporte Deporte { get; set; } = null!;

        // N EventoDeportivo → 1 TipoCompetencia
        public Guid TipoCompetenciaId { get; set; }
        public TipoCompetencia TipoCompetencia { get; set; } = null!;

        // 1 EventoDeportivo → N Partidos
        public ICollection<Partido> Partidos { get; set; } = new List<Partido>();

        // 1 EventoDeportivo → N PlantillasPenca
        public ICollection<PlantillaPenca> Plantillas { get; set; } = new List<PlantillaPenca>();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class PlantillaPenca : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;

        // N PlantillasPenca → 1 EventoDeportivo
        public Guid EventoDeportivoId { get; set; }
        public EventoDeportivo Evento { get; set; } = null!;

        // 1 PlantillaPenca → N ReglasPuntuacion
        public ICollection<ReglaPuntuacion> Reglas { get; set; } = new List<ReglaPuntuacion>();

        // 1 PlantillaPenca → N Pencas
        public ICollection<Penca> Pencas { get; set; } = new List<Penca>();
    }
}

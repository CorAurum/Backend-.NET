using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class Deporte : BaseEntity
    {
        public string Nombre { get; set; } = null!;

        // 1 Deporte → N EventoDeportivo
        public ICollection<EventoDeportivo> Eventos { get; set; } = new List<EventoDeportivo>();
    }
}

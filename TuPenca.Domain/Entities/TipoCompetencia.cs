using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class TipoCompetencia : BaseEntity
    {
        public string Nombre { get; set; } = null!;

        // 1 TipoCompetencia → N EventoDeportivo
        public ICollection<EventoDeportivo> Eventos { get; set; } = new List<EventoDeportivo>();
    }
}

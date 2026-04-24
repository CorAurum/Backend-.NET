using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class Equipo : BaseEntity
    {
        public string Nombre { get; set; } = null!;

        // N Equipos → N Partidos (como local o visitante, dos FKs separadas)
        // ⚠️ Requiere configuración especial en AppDbContext (InverseProperty)
        public ICollection<Partido> PartidosComoLocal { get; set; } = new List<Partido>();
        public ICollection<Partido> PartidosComoVisitante { get; set; } = new List<Partido>();
    }

}

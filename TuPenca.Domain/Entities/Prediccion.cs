using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class Prediccion : BaseEntity
    {
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }

        // N Predicciones → 1 Usuario
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // N Predicciones → 1 Partido
        public Guid PartidoId { get; set; }
        public Partido Partido { get; set; } = null!;
    }
}

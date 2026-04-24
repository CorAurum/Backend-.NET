using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class PuntajeUsuario : BaseEntity
    {
        public int PuntosTotales { get; set; }

        // N PuntajesUsuario → 1 Usuario
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // N PuntajesUsuario → 1 Penca
        public Guid PencaId { get; set; }
        public Penca Penca { get; set; } = null!;
    }
}

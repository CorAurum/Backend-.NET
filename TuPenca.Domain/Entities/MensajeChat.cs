using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class MensajeChat : BaseEntity
    {
        public string Contenido { get; set; } = null!;
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        // N Mensajes → 1 Usuario (autor)
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // N Mensajes → 1 Penca (foro de esa penca)
        public Guid PencaId { get; set; }
        public Penca Penca { get; set; } = null!;
    }
}

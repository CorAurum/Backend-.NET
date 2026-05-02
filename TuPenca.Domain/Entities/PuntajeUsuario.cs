using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class PuntajeUsuario : BaseEntity
    {
        public int PuntosPartido { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public Guid PencaId { get; set; }
        public Penca Penca { get; set; } = null!;
        public Guid PartidoId { get; set; }
        public Partido Partido { get; set; } = null!;
    }
}

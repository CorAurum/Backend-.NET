using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class Premio : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
        public int Monto { get; set; }
        public int Posicion { get; set; }

        // N Premios → 1 Penca
        public Guid PencaId { get; set; }
        public Penca Penca { get; set; } = null!;

        // atributos para premios

        public Guid? UsuarioGanadorId { get; set; }
        public Usuario? UsuarioGanador { get; set; }
    }
}

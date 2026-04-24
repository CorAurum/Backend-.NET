using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class Notificacion : BaseEntity
    {
        public string Tipo { get; set; } = null!; // "Recordatorio", "Resultado", "ResumenSemanal"
        public string Mensaje { get; set; } = null!;
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; } = false;

        // N Notificaciones → 1 Usuario
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // N Notificaciones → 1 Penca
        public Guid PencaId { get; set; }
        public Penca Penca { get; set; } = null!;
    }
}

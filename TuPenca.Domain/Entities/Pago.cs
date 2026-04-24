using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Enums;

namespace TuPenca.Domain.Entities
{
    public class Pago : BaseEntity
    {
        public int Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;
 

        // N Pagos → 1 Usuario
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}

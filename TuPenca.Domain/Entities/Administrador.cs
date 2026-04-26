using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class Administrador : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // N Administradores → 1 Sitio Revisar esta relacion si corresponde, da trazabilidad a que administrador aprobo que sitio
        //public Guid SitioId { get; set; }
        //public Sitio Sitio { get; set; } = null!;

        // 1 Administrador → N Invitaciones generadas
        public ICollection<Invitacion> Invitaciones { get; set; } = new List<Invitacion>();
    }
}

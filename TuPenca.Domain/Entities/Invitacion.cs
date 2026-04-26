using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class Invitacion : BaseEntity
    {
        public string EmailInvitado { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public bool Aceptada { get; set; } = false;

        // N Invitaciones → 1 Sitio
        public Guid SitioId { get; set; }
        public Sitio Sitio { get; set; } = null!;

        // N Invitaciones → 1 Administrador de sitio (quien la generó)
        public Guid UsuarioId { get; set; }      // el AdminSitio que generó la invitación
        public Usuario Usuario { get; set; } = null!;
    }

}

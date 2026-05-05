using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Invitacion
{
    public class InvitacionResponseDto
    {
        public Guid Id { get; set; }
        public string EmailInvitado { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public bool Aceptada { get; set; }
    }
}
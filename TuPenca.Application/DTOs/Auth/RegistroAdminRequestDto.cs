using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Auth
{
    public class RegistroAdminRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
//        public Guid SitioId { get; set; }
    }
}

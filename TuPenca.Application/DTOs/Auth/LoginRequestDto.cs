using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public Guid? SitioId { get; set; } // nullable porque Administrador no lo necesita
    }
}

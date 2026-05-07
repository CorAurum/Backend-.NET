using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Usuario
{
    public class UsuarioActualizarPasswordRequestDto
    {
        public Guid Id { get; set; }
        public string Password { get; set; } = null!;
        public string PasswordConfirm { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
    }
}

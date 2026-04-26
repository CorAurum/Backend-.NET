using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Auth
{
    public class RegistroResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Mensaje { get; set; } = null!; // ej: "Registro exitoso, pendiente de aprobación"
    }
}

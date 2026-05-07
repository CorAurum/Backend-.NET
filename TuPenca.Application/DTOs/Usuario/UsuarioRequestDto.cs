using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Usuario
{
    public class UsuarioRequestDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public byte[]? Foto { get; set; } = null;
    }
}

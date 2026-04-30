using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Equipo
{
    public class EquipoResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}

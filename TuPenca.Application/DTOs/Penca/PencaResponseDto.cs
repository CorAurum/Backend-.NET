using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Penca
{
    public class PencaResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public EstadoPenca Estado { get; set; }
        public string PlantillaNombre { get; set; } = null!;
        public string EventoDeportivo { get; set; } = null!;
    }
}
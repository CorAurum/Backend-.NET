using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.PlantillaPenca
{
    public class PlantillaPencaResponseDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int TiempoLimitePrevioMinutos { get; set; }
        public string EventoDeportivo { get; set; } = null!;
        public List<ReglaPuntuacionDto> Reglas { get; set; } = new();
    }
}

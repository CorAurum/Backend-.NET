using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class ReglaPuntuacion : BaseEntity
    {
        public string TipoAcierto { get; set; } = null!; // ej: "ResultadoExacto", "GanadorCorrecto"
        public int Puntaje { get; set; }

        // N Reglas → 1 PlantillaPenca
        public Guid PlantillaPencaId { get; set; }
        public PlantillaPenca Plantilla { get; set; } = null!;
    }
}

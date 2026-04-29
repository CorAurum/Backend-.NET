using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Domain.Entities
{
    public class ReglaPuntuacion : BaseEntity
    {
        // 0 = exacto, 1 = te desviaste 1 gol en total, etc.
        public int Desviacion { get; set; }
        public int Puntaje { get; set; }

        // N Reglas → 1 PlantillaPenca
        public Guid PlantillaPencaId { get; set; }
        public PlantillaPenca Plantilla { get; set; } = null!; 
    }
}

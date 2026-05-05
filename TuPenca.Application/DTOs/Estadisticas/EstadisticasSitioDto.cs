using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Estadisticas
{
    public class EstadisticasSitioDto
    {
        public string NombreSitio { get; set; } = null!;
        public int TotalUsuarios { get; set; }
        public int TotalPencasActivas { get; set; }
        public int TotalPencasFinalizadas { get; set; }

        public int TotalComisionesGeneradas { get; set; }
        public int TotalRecaudado { get; set; }
        public List<EstadisticaPencaDto> EstadisticasPorPenca { get; set; } = new();
    }

    public class EstadisticaPencaDto
    {
        public string NombrePenca { get; set; } = null!;
        public string LiderActual { get; set; } = null!;
        public int PuntosLider { get; set; }
        public int TotalParticipantes { get; set; }
        public int TotalPartidosConPrediccion { get; set; }
    }
}
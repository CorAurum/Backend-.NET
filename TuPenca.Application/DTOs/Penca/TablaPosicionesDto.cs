using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Penca
{
    public class TablaPosicionesDto
    {
        public Guid PencaId { get; set; }
        public string NombrePenca { get; set; } = null!;
        public List<PosicionUsuarioDto> Posiciones { get; set; } = new();
    }

    public class PosicionUsuarioDto
    {
        public int Posicion { get; set; }
        public Guid UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public int PuntosTotales { get; set; }
        public int PartidosPredichos { get; set; }
    }
}
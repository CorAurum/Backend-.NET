using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Enums;

namespace TuPenca.Domain.Entities
{
    public class Penca : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public EstadoPenca Estado { get; set; } = EstadoPenca.Abierta;

        // N Pencas → 1 PlantillaPenca
        public Guid PlantillaPencaId { get; set; }
        public PlantillaPenca Plantilla { get; set; } = null!;

        // N Pencas → 1 Sitio
        public Guid SitioId { get; set; }
        public Sitio Sitio { get; set; } = null!;

        // 1 Penca → N Premios
        public ICollection<Premio> Premios { get; set; } = new List<Premio>();

        // 1 Penca → N PuntajesUsuario
        public ICollection<PuntajeUsuario> Puntajes { get; set; } = new List<PuntajeUsuario>();

        // 1 Penca → N MensajesChat
        public ICollection<MensajeChat> Mensajes { get; set; } = new List<MensajeChat>();

        // 1 Penca → N Notificaciones
        public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    }
}

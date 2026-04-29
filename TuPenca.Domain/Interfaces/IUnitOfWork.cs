using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces.Repositories;

namespace TuPenca.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUsuarioRepository Usuarios { get; }
        IAdministradorRepository Administrador { get; }
        IRepository<Sitio> Sitios { get; }
        IPencaRepository Pencas { get; }
        IRepository<Partido> Partidos { get; }
        IRepository<Prediccion> Predicciones { get; }
        IRepository<PuntajeUsuario> PuntajesUsuario { get; }
        IRepository<Premio> Premios { get; }
        IRepository<Pago> Pagos { get; }
        IRepository<MensajeChat> MensajesChat { get; }
        IRepository<Notificacion> Notificaciones { get; }
        IRepository<Invitacion> Invitaciones { get; }
        IPlantillaPencaRepository PlantillasPenca { get; }
        IRepository<EventoDeportivo> EventosDeportivos { get; }
        IRepository<Equipo> Equipos { get; }

        Task<int> SaveChangesAsync(); // ← confirma todos los cambios pendientes
    }
}

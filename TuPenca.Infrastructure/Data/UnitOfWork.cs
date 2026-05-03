using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;
using TuPenca.Domain.Interfaces.Repositories;
using TuPenca.Infrastructure.Data.Repositories;

namespace TuPenca.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUsuarioRepository Usuarios { get; }
        public IAdministradorRepository Administrador { get; }
        public IRepository<Sitio> Sitios { get; }
        public IPencaRepository Pencas { get; }
        public IRepository<Partido> Partidos { get; }
        public IPrediccionRepository Predicciones { get; }
        public IPuntajeUsuarioRepository PuntajesUsuario { get; }
        public IPremioRepository Premios { get; }
        public IRepository<Pago> Pagos { get; }
        public IRepository<MensajeChat> MensajesChat { get; }
        public IRepository<Notificacion> Notificaciones { get; }
        public IRepository<Invitacion> Invitaciones { get; }
        public IPlantillaPencaRepository PlantillasPenca { get; }
        public IRepository<EventoDeportivo> EventosDeportivos { get; }
        public IRepository<Equipo> Equipos { get; }

        public UnitOfWork(AppDbContext context, IUsuarioRepository usuarios, IAdministradorRepository administradores, IPlantillaPencaRepository plantillaPenca, 
            IPencaRepository pencas, IPrediccionRepository Prediccion, IPuntajeUsuarioRepository PuntajesUsuarios, IPremioRepository Premio)
        {
            _context = context;
            Usuarios = usuarios;
            Administrador = administradores;
            Sitios = new Repository<Sitio>(context);
            Pencas = pencas;
            Partidos = new Repository<Partido>(context);
            Predicciones = Prediccion;
            PuntajesUsuario = PuntajesUsuarios;
            Premios = Premio;
            Pagos = new Repository<Pago>(context);
            MensajesChat = new Repository<MensajeChat>(context);
            Notificaciones = new Repository<Notificacion>(context);
            Invitaciones = new Repository<Invitacion>(context);
            PlantillasPenca = plantillaPenca;
            EventosDeportivos = new Repository<EventoDeportivo>(context);
            Equipos = new Repository<Equipo>(context);
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}

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
        public IRepository<Penca> Pencas { get; }
        public IRepository<Partido> Partidos { get; }
        public IRepository<Prediccion> Predicciones { get; }
        public IRepository<PuntajeUsuario> PuntajesUsuario { get; }
        public IRepository<Premio> Premios { get; }
        public IRepository<Pago> Pagos { get; }
        public IRepository<MensajeChat> MensajesChat { get; }
        public IRepository<Notificacion> Notificaciones { get; }
        public IRepository<Invitacion> Invitaciones { get; }
        public IRepository<PlantillaPenca> PlantillasPenca { get; }
        public IRepository<EventoDeportivo> EventosDeportivos { get; }
        public IRepository<Equipo> Equipos { get; }

        public UnitOfWork(AppDbContext context, IUsuarioRepository usuarios, IAdministradorRepository administradores)
        {
            _context = context;
            Usuarios = usuarios;
            Administrador = administradores;
            Sitios = new Repository<Sitio>(context);
            Pencas = new Repository<Penca>(context);
            Partidos = new Repository<Partido>(context);
            Predicciones = new Repository<Prediccion>(context);
            PuntajesUsuario = new Repository<PuntajeUsuario>(context);
            Premios = new Repository<Premio>(context);
            Pagos = new Repository<Pago>(context);
            MensajesChat = new Repository<MensajeChat>(context);
            Notificaciones = new Repository<Notificacion>(context);
            Invitaciones = new Repository<Invitacion>(context);
            PlantillasPenca = new Repository<PlantillaPenca>(context);
            EventosDeportivos = new Repository<EventoDeportivo>(context);
            Equipos = new Repository<Equipo>(context);
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}

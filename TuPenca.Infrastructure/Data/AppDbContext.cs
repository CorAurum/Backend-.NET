using Microsoft.EntityFrameworkCore;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ─── DbSets ───────────────────────────────────────────────
    // Equivalente a los @Repository en Spring — cada uno representa una tabla
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Administrador> Administradores => Set<Administrador>();
    public DbSet<Sitio> Sitios => Set<Sitio>();
    public DbSet<Penca> Pencas => Set<Penca>();
    public DbSet<PlantillaPenca> PlantillasPenca => Set<PlantillaPenca>();
    public DbSet<ReglaPuntuacion> ReglasPuntuacion => Set<ReglaPuntuacion>();
    public DbSet<EventoDeportivo> EventosDeportivos => Set<EventoDeportivo>();
    public DbSet<Partido> Partidos => Set<Partido>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Deporte> Deportes => Set<Deporte>();
    public DbSet<TipoCompetencia> TiposCompetencia => Set<TipoCompetencia>();
    public DbSet<Prediccion> Predicciones => Set<Prediccion>();
    public DbSet<PuntajeUsuario> PuntajesUsuario => Set<PuntajeUsuario>();
    public DbSet<Premio> Premios => Set<Premio>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<MensajeChat> MensajesChat => Set<MensajeChat>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<Invitacion> Invitaciones => Set<Invitacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ─── Partido: dos FKs al mismo Equipo ─────────────────
        // ⚠️ Esto es el caso especial que mencionamos antes
        // EF no sabe cuál FK es local y cuál visitante sin aclarárselo
        modelBuilder.Entity<Partido>()
            .HasOne(p => p.EquipoLocal)
            .WithMany(e => e.PartidosComoLocal)
            .HasForeignKey(p => p.EquipoLocalId)
            .OnDelete(DeleteBehavior.Restrict); // Restrict evita borrado en cascada

        modelBuilder.Entity<Partido>()
            .HasOne(p => p.EquipoVisitante)
            .WithMany(e => e.PartidosComoVisitante)
            .HasForeignKey(p => p.EquipoVisitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        // ─── Enums como string en la BD ────────────────────────
        // Por defecto EF guarda enums como int (0,1,2)
        // Esto los guarda como "Pendiente", "Aprobado" — más legible
        modelBuilder.Entity<Usuario>()
            .Property(u => u.Estado)
            .HasConversion<string>();

        modelBuilder.Entity<Usuario>()
            .Property(u => u.ProveedorAuth)
            .HasConversion<string>();

        modelBuilder.Entity<Sitio>()
            .Property(s => s.Estado)
            .HasConversion<string>();

        modelBuilder.Entity<Pago>()
            .Property(p => p.Estado)
            .HasConversion<string>();

        //modelBuilder.Entity<Pago>()
        //    .Property(p => p.MetodoPago)
        //    .HasConversion<string>();

        // ─── Índices únicos ────────────────────────────────────
        // Equivalente a @Column(unique=true) en JPA
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => new { u.Email, u.SitioId })
            .IsUnique(); // El email es único por sitio, no globalmente

        modelBuilder.Entity<Sitio>()
            .HasIndex(s => s.UrlPropia)
            .IsUnique();

        modelBuilder.Entity<Invitacion>()
            .HasIndex(i => i.Codigo)
            .IsUnique();


        // experimental
        modelBuilder.Entity<Invitacion>()
    .HasOne(i => i.Sitio)
    .WithMany(s => s.Invitaciones)
    .HasForeignKey(i => i.SitioId)
    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Invitacion>()
    .HasOne(i => i.Administrador)
    .WithMany(a => a.Invitaciones)
    .HasForeignKey(i => i.AdministradorId)
    .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<MensajeChat>()
    .HasOne(m => m.Usuario)
    .WithMany(u => u.Mensajes)
    .HasForeignKey(m => m.UsuarioId)
    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MensajeChat>()
            .HasOne(m => m.Penca)
            .WithMany(p => p.Mensajes)
            .HasForeignKey(m => m.PencaId)
          .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Notificacion>()
    .HasOne(n => n.Penca)
    .WithMany(p => p.Notificaciones)
    .HasForeignKey(n => n.PencaId)
    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Notificacion>()
    .HasOne(n => n.Usuario)
    .WithMany(u => u.Notificaciones)
    .HasForeignKey(n => n.UsuarioId)
    .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<PuntajeUsuario>()
    .HasOne(p => p.Penca)
    .WithMany(p => p.Puntajes)
    .HasForeignKey(p => p.PencaId)
    .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<PuntajeUsuario>()
    .HasOne(p => p.Usuario)
    .WithMany(u => u.Puntajes)
    .HasForeignKey(p => p.UsuarioId)
    .OnDelete(DeleteBehavior.NoAction);

    }
}
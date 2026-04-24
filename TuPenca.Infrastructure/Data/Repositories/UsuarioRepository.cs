using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TuPenca.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public async Task<Usuario?> GetByEmailAsync(string email, Guid sitioId)
            => await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email && u.SitioId == sitioId);

        public async Task<IEnumerable<Usuario>> GetBySitioAsync(Guid sitioId)
            => await _dbSet
                .Where(u => u.SitioId == sitioId)
                .ToListAsync();

        public async Task<IEnumerable<Usuario>> GetPendientesAprobacionAsync(Guid sitioId)
            => await _dbSet
                .Where(u => u.SitioId == sitioId && u.Estado == EstadoUsuario.Pendiente)
                .ToListAsync();
    }
}

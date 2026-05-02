using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces.Repositories;

namespace TuPenca.Infrastructure.Data.Repositories
{
    public class PuntajeUsuarioRepository : Repository<PuntajeUsuario>, IPuntajeUsuarioRepository
    {
        public PuntajeUsuarioRepository(AppDbContext context) : base(context) { }

        public async Task<PuntajeUsuario?> GetByUsuarioPencaPartidoAsync(Guid usuarioId, Guid pencaId, Guid partidoId)
            => await _context.PuntajesUsuario
                .FirstOrDefaultAsync(p =>
                    p.UsuarioId == usuarioId &&
                    p.PencaId == pencaId &&
                    p.PartidoId == partidoId);

        public async Task<IEnumerable<PuntajeUsuario>> GetByPencaAsync(Guid pencaId)
            => await _context.PuntajesUsuario
                .Include(p => p.Usuario)
                .Where(p => p.PencaId == pencaId)
                .ToListAsync();
    }
}
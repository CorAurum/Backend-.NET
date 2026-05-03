using Microsoft.EntityFrameworkCore;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces.Repositories;

namespace TuPenca.Infrastructure.Data.Repositories
{
    public class PremioRepository : Repository<Premio>, IPremioRepository
    {
        public PremioRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Premio>> GetByPencaAsync(Guid pencaId)
            => await _context.Premios
                .Include(p => p.UsuarioGanador)
                .Where(p => p.PencaId == pencaId)
                .OrderBy(p => p.Posicion)
                .ToListAsync();
    }
}
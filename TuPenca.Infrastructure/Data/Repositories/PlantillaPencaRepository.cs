using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces.Repositories;

namespace TuPenca.Infrastructure.Data.Repositories
{
    public class PlantillaPencaRepository : Repository<PlantillaPenca>, IPlantillaPencaRepository
    {
        public PlantillaPencaRepository(AppDbContext context) : base(context) { }

        public async Task<PlantillaPenca?> GetByIdConDetalleAsync(Guid id)
            => await _context.PlantillasPenca
                .Include(p => p.Evento)
                .Include(p => p.Reglas)
                .Include(p => p.Pencas)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<PlantillaPenca>> GetAllConDetalleAsync()
            => await _context.PlantillasPenca
                .Include(p => p.Evento)
                .Include(p => p.Reglas)
                .ToListAsync();
    }
}
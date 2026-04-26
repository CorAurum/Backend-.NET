using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TuPenca.Infrastructure.Data.Repositories
{
    public class AdministradorRepository : Repository<Administrador>, IAdministradorRepository
    {
        public AdministradorRepository(AppDbContext context) : base(context) { }

        public async Task<Administrador?> GetByEmailAsync(string email)
            => await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);

    }
}

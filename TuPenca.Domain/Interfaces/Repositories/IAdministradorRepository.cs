using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Entities;

namespace TuPenca.Domain.Interfaces.Repositories
{
    public interface IAdministradorRepository : IRepository<Administrador>
    {
        // Métodos específicos que el genérico no tiene
        Task<Administrador?> GetByEmailAsync(string email, Guid sitioId);
        Task<IEnumerable<Administrador>> GetBySitioAsync(Guid sitioId);
    
    }
}

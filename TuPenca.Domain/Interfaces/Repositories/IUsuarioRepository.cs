using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Domain.Entities;

namespace TuPenca.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        // Métodos específicos que el genérico no tiene
        Task<Usuario?> GetByEmailAsync(string email, Guid sitioId);
        Task<IEnumerable<Usuario>> GetBySitioAsync(Guid sitioId);
        Task<IEnumerable<Usuario>> GetPendientesAprobacionAsync(Guid sitioId);
    }
}

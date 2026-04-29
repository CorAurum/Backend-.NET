using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Domain.Entities;

namespace TuPenca.Domain.Interfaces.Repositories
{
    public interface IPencaRepository : IRepository<Penca>
    {
        Task<Penca?> GetByIdConDetalleAsync(Guid id);
        Task<IEnumerable<Penca>> GetAllConDetalleAsync();
    }
}
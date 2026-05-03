using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Domain.Entities;

namespace TuPenca.Domain.Interfaces.Repositories
{
    public interface IPremioRepository : IRepository<Premio>
    {
        Task<IEnumerable<Premio>> GetByPencaAsync(Guid pencaId);
    }
}
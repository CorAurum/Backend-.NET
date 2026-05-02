using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Domain.Entities;

namespace TuPenca.Domain.Interfaces.Repositories
{
    public interface IPrediccionRepository : IRepository<Prediccion>
    {
        Task<IEnumerable<Prediccion>> GetByPartidoConDetalleAsync(Guid partidoId);
    }
}
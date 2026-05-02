using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Domain.Entities;

namespace TuPenca.Domain.Interfaces.Repositories
{
    public interface IPuntajeUsuarioRepository : IRepository<PuntajeUsuario>
    {
        Task<PuntajeUsuario?> GetByUsuarioPencaPartidoAsync(Guid usuarioId, Guid pencaId, Guid partidoId);
        Task<IEnumerable<PuntajeUsuario>> GetByPencaAsync(Guid pencaId);
    }
}
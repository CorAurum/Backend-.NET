using TuPenca.Domain.Entities;

namespace TuPenca.Domain.Interfaces.Repositories
{
    public interface IPlantillaPencaRepository : IRepository<PlantillaPenca>
    {
        Task<PlantillaPenca?> GetByIdConDetalleAsync(Guid id);
        Task<IEnumerable<PlantillaPenca>> GetAllConDetalleAsync();
    }
}
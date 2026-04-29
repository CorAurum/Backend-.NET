using TuPenca.Application.DTOs.PlantillaPenca;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IPlantillaPencaService
    {
        Task<IEnumerable<PlantillaPencaResponseDto>> ObtenerTodasAsync();
        Task<PlantillaPencaResponseDto?> ObtenerPorIdAsync(Guid id);
        Task<PlantillaPencaResponseDto> CrearAsync(PlantillaPencaRequestDto dto);
        Task EliminarAsync(Guid id);
    }
}
using TuPenca.Application.DTOs.Deporte;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IDeporteService
    {
        Task<IEnumerable<DeporteResponseDto>> ObtenerTodosAsync();
        Task<IEnumerable<DeporteResponseDto>> CrearVariosAsync(DeporteRequestDto dto);
    }
}

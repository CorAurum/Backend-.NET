using TuPenca.Application.DTOs.TipoCompetencia;

namespace TuPenca.Application.Interfaces.Services
{
    public interface ITipoCompetenciaService
    {
        Task<IEnumerable<TipoCompetenciaResponseDto>> ObtenerTodosAsync();
        Task<IEnumerable<TipoCompetenciaResponseDto>> CrearVariosAsync(TipoCompetenciaRequestDto dto);
    }
}

using TuPenca.Application.DTOs.Sitio;

namespace TuPenca.Application.Interfaces.Services
{
    public interface ISitioService
    {
        Task<IEnumerable<SitioDto>> ObtenerSitiosAsync();

        Task<SitioDto> ObtenerSitioAsync(Guid sitioId);

        Task<SitioResponseDto> CrearSitioAsync(SitioRequestDto sitioDto);

        Task<SitioResponseDto> ActualizarSitioAsync(SitioRequestDto sitioDto);

        Task<SitioResponseDto> EliminarSitioAsync(Guid sitioId);
    }
}

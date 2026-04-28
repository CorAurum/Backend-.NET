using TuPenca.Application.DTOs.Sitio;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class SitioService : ISitioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SitioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SitioDto>> ObtenerSitiosAsync()
        {
            var sitios = await _unitOfWork.Sitios.GetAllAsync();
            var result = new List<SitioDto>();
            foreach (var sitio in sitios)
            {
                result.Add(new SitioDto()
                {
                    Id = sitio.Id,
                    Nombre = sitio.Nombre,
                    UrlPropia = sitio.UrlPropia,
                    ConfiguracionSitio = sitio.ConfiguracionSitio,
                    EsquemaColores = sitio.EsquemaColores,
                    TipoRegistro = sitio.TipoRegistro
                });
            }
            return result;
        }

        public async Task<SitioDto> ObtenerSitioAsync(Guid sitioId)
        {
            var sitio = await _unitOfWork.Sitios.GetByIdAsync(sitioId);
            return new SitioDto()
            {
                Id = sitio.Id,
                Nombre = sitio.Nombre,
                UrlPropia = sitio.UrlPropia,
                ConfiguracionSitio = sitio.ConfiguracionSitio,
                EsquemaColores = sitio.EsquemaColores,
                TipoRegistro = sitio.TipoRegistro
            };
        }

        public async Task<SitioResponseDto> CrearSitioAsync(SitioRequestDto sitioDto)
        {
            sitioDto.Id = Guid.NewGuid();

            var sitio = new Sitio()
            {
                Id = sitioDto.Id,
                Nombre = sitioDto.Nombre,
                UrlPropia = sitioDto.UrlPropia,
                ConfiguracionSitio = sitioDto.ConfiguracionSitio,
                EsquemaColores = sitioDto.EsquemaColores,
                TipoRegistro = sitioDto.TipoRegistro
            };

            await _unitOfWork.Sitios.AddAsync(sitio);
            await _unitOfWork.SaveChangesAsync();

            return new SitioResponseDto()
            {
                Id = sitio.Id,
                Nombre = sitio.Nombre,
                Mensaje = "Sitio creado exitosamente"
            };
        }

        public async Task<SitioResponseDto> ActualizarSitioAsync(SitioRequestDto sitioDto)
        {
            var sitio = await _unitOfWork.Sitios.GetByIdAsync(sitioDto.Id);
            if (sitio == null)
                return new SitioResponseDto { Mensaje = "Sitio no encontrado" };

            sitio.Nombre = sitioDto.Nombre;
            sitio.UrlPropia = sitioDto.UrlPropia;
            sitio.ConfiguracionSitio = sitioDto.ConfiguracionSitio;
            sitio.EsquemaColores = sitioDto.EsquemaColores;
            sitio.TipoRegistro = sitioDto.TipoRegistro;

            await _unitOfWork.Sitios.UpdateAsync(sitio);
            await _unitOfWork.SaveChangesAsync();

            return new SitioResponseDto
            {
                Id = sitio.Id,
                Nombre = sitio.Nombre,
                Mensaje = "Sitio actualizado exitosamente"
            };
        }

        public async Task<SitioResponseDto> EliminarSitioAsync(Guid sitioId)
        {
            var sitio = await _unitOfWork.Sitios.GetByIdAsync(sitioId);

            await _unitOfWork.Sitios.DeleteAsync(sitioId);
            await _unitOfWork.SaveChangesAsync();

            return new SitioResponseDto()
            {
                Id = sitio.Id,
                Nombre = sitio.Nombre,
                Mensaje = "Sitio creado exitosamente"
            };
        }
    }
}

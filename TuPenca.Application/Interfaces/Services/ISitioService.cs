using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Application.DTOs.Sitio;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Interfaces.Services
{
    public interface ISitioService
    {
        Task<IEnumerable<SitioResponseDto>> ObtenerSitiosAsync();

        Task<SitioResponseDto> ObtenerSitioAsync(Guid sitioId);

        Task<SitioResponseDto> CrearSitioAsync(SitioRequestDto sitioDto);
    }
}

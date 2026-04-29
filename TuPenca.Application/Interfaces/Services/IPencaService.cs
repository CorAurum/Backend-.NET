using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Application.DTOs.Penca;
using TuPenca.Domain.Enums;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IPencaService
    {
        Task<IEnumerable<PencaResponseDto>> ObtenerTodasAsync();
        Task<PencaResponseDto?> ObtenerPorIdAsync(Guid id);
        Task<PencaResponseDto> CrearAsync(PencaRequestDto dto, Guid sitioId);
        Task<PencaResponseDto> CambiarEstadoAsync(Guid id, EstadoPenca nuevoEstado);
        Task EliminarAsync(Guid id);
    }
}
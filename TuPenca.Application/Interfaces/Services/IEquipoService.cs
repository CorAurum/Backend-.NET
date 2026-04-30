using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Application.DTOs.Equipo;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IEquipoService
    {
        Task<IEnumerable<EquipoResponseDto>> ObtenerTodosAsync();
        Task<IEnumerable<EquipoResponseDto>> CrearVariosAsync(EquipoRequestDto dto);
    }
}
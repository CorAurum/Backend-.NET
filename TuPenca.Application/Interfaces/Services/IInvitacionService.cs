using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Application.DTOs.Invitacion;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IInvitacionService
    {
        Task<InvitacionResponseDto> GenerarInvitacionAsync(InvitacionRequestDto dto, Guid usuarioId, Guid sitioId);
        Task<IEnumerable<InvitacionResponseDto>> ObtenerInvitacionesSitioAsync(Guid sitioId);
    }
}
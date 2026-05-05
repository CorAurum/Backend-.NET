using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Application.DTOs.Invitacion;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class InvitacionService : IInvitacionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvitacionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InvitacionResponseDto> GenerarInvitacionAsync(InvitacionRequestDto dto, Guid usuarioId, Guid sitioId)
        {
            var sitio = await _unitOfWork.Sitios.GetByIdAsync(sitioId);
            if (sitio == null)
                throw new Exception("Sitio no encontrado");

            if (sitio.TipoRegistro != TipoRegistro.Con_Invitacion)
                throw new Exception("Este sitio no usa invitaciones");

            // Verificar que no exista una invitación pendiente para ese email
            var invitaciones = await _unitOfWork.Invitaciones.GetAllAsync();
            var invitacionExistente = invitaciones.FirstOrDefault(i =>
                i.EmailInvitado == dto.EmailInvitado &&
                i.SitioId == sitioId &&
                !i.Aceptada);

            if (invitacionExistente != null)
                throw new Exception("Ya existe una invitación pendiente para ese email");

            var invitacion = new Invitacion
            {
                Id = Guid.NewGuid(),
                EmailInvitado = dto.EmailInvitado,
                Codigo = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                Aceptada = false,
                SitioId = sitioId,
                UsuarioId = usuarioId
            };

            await _unitOfWork.Invitaciones.AddAsync(invitacion);
            await _unitOfWork.SaveChangesAsync();

            return new InvitacionResponseDto
            {
                Id = invitacion.Id,
                EmailInvitado = invitacion.EmailInvitado,
                Codigo = invitacion.Codigo,
                Aceptada = invitacion.Aceptada
            };
        }

        public async Task<IEnumerable<InvitacionResponseDto>> ObtenerInvitacionesSitioAsync(Guid sitioId)
        {
            var invitaciones = await _unitOfWork.Invitaciones.GetAllAsync();
            return invitaciones
                .Where(i => i.SitioId == sitioId)
                .Select(i => new InvitacionResponseDto
                {
                    Id = i.Id,
                    EmailInvitado = i.EmailInvitado,
                    Codigo = i.Codigo,
                    Aceptada = i.Aceptada
                });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Application.DTOs.Pago;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class PagoService : IPagoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagoResponseDto> RealizarPagoAsync(PagoRequestDto dto, Guid usuarioId)
        {
            var penca = await _unitOfWork.Pencas.GetByIdAsync(dto.PencaId);
            if (penca == null)
                throw new Exception("Penca no encontrada");

            if (penca.Estado != EstadoPenca.Abierta)
                throw new Exception("La penca no está abierta para nuevos participantes");

            // Verificar que no haya pagado antes
            var pagos = await _unitOfWork.Pagos.GetAllAsync();
            var pagoExistente = pagos.FirstOrDefault(p =>
                p.UsuarioId == usuarioId &&
                p.PencaId == dto.PencaId &&
                p.Estado == EstadoPago.Aprobado);

            if (pagoExistente != null)
                throw new Exception("Ya estás inscripto en esta penca");

            // Por ahora simulamos pago exitoso
            var pago = new Pago
            {
                Id = Guid.NewGuid(),
                Monto = dto.Monto,
                Fecha = DateTime.UtcNow,
                Estado = EstadoPago.Aprobado,
                UsuarioId = usuarioId,
                PencaId = dto.PencaId
            };

            await _unitOfWork.Pagos.AddAsync(pago);
            await _unitOfWork.SaveChangesAsync();

            return new PagoResponseDto
            {
                Id = pago.Id,
                PencaId = pago.PencaId,
                UsuarioId = pago.UsuarioId,
                Monto = pago.Monto,
                Estado = pago.Estado,
                Fecha = pago.Fecha
            };
        }

        public async Task<bool> UsuarioPagoEnPencaAsync(Guid usuarioId, Guid pencaId)
        {
            var pagos = await _unitOfWork.Pagos.GetAllAsync();
            return pagos.Any(p =>
                p.UsuarioId == usuarioId &&
                p.PencaId == pencaId &&
                p.Estado == EstadoPago.Aprobado);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Application.DTOs.Penca;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class PencaService : IPencaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PencaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PencaResponseDto>> ObtenerTodasAsync()
        {
            var pencas = await _unitOfWork.Pencas.GetAllConDetalleAsync();
            return pencas.Select(p => new PencaResponseDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Estado = p.Estado,
                PlantillaNombre = p.Plantilla?.Nombre ?? string.Empty,
                EventoDeportivo = p.Plantilla?.Evento?.Nombre ?? string.Empty
            });
        }

        public async Task<PencaResponseDto?> ObtenerPorIdAsync(Guid id)
        {
            var penca = await _unitOfWork.Pencas.GetByIdConDetalleAsync(id);
            if (penca == null) return null;

            return new PencaResponseDto
            {
                Id = penca.Id,
                Nombre = penca.Nombre,
                Estado = penca.Estado,
                PlantillaNombre = penca.Plantilla?.Nombre ?? string.Empty,
                EventoDeportivo = penca.Plantilla?.Evento?.Nombre ?? string.Empty
            };
        }

        public async Task<PencaResponseDto> CrearAsync(PencaRequestDto dto, Guid sitioId)
        {
            var plantilla = await _unitOfWork.PlantillasPenca.GetByIdConDetalleAsync(dto.PlantillaPencaId);
            if (plantilla == null)
                throw new Exception("Plantilla no encontrada");

            var penca = new Penca
            {
                Id = Guid.NewGuid(),
                Nombre = dto.Nombre,
                Estado = EstadoPenca.Abierta,
                PlantillaPencaId = dto.PlantillaPencaId,
                SitioId = sitioId
            };

            await _unitOfWork.Pencas.AddAsync(penca);
            await _unitOfWork.SaveChangesAsync();

            return new PencaResponseDto
            {
                Id = penca.Id,
                Nombre = penca.Nombre,
                Estado = penca.Estado,
                PlantillaNombre = plantilla.Nombre,
                EventoDeportivo = plantilla.Evento?.Nombre ?? string.Empty
            };
        }

        public async Task<PencaResponseDto> CambiarEstadoAsync(Guid id, EstadoPenca nuevoEstado)
        {
            var penca = await _unitOfWork.Pencas.GetByIdConDetalleAsync(id);
            if (penca == null)
                throw new Exception("Penca no encontrada");

            penca.Estado = nuevoEstado;
            await _unitOfWork.Pencas.UpdateAsync(penca);
            await _unitOfWork.SaveChangesAsync();

            return new PencaResponseDto
            {
                Id = penca.Id,
                Nombre = penca.Nombre,
                Estado = penca.Estado,
                PlantillaNombre = penca.Plantilla?.Nombre ?? string.Empty,
                EventoDeportivo = penca.Plantilla?.Evento?.Nombre ?? string.Empty
            };
        }

        public async Task EliminarAsync(Guid id)
        {
            var penca = await _unitOfWork.Pencas.GetByIdAsync(id);
            if (penca == null)
                throw new Exception("Penca no encontrada");

            if (penca.Estado == EstadoPenca.EnCurso)
                throw new Exception("No se puede eliminar una penca que está en curso");

            await _unitOfWork.Pencas.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
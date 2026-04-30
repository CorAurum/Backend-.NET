using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Application.DTOs.Equipo;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class EquipoService : IEquipoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EquipoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EquipoResponseDto>> ObtenerTodosAsync()
        {
            var equipos = await _unitOfWork.Equipos.GetAllAsync();
            return equipos.Select(e => new EquipoResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre
            });
        }

        public async Task<IEnumerable<EquipoResponseDto>> CrearVariosAsync(EquipoRequestDto dto)
        {
            if (!dto.Nombres.Any())
                throw new Exception("Debe enviar al menos un equipo");

            var equipos = dto.Nombres.Select(nombre => new Equipo
            {
                Id = Guid.NewGuid(),
                Nombre = nombre
            }).ToList();

            foreach (var equipo in equipos)
                await _unitOfWork.Equipos.AddAsync(equipo);

            await _unitOfWork.SaveChangesAsync();

            return equipos.Select(e => new EquipoResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre
            });
        }
    }
}
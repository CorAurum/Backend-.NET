using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Application.DTOs.Prediccion;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class PrediccionService : IPrediccionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPagoService _pagoService;

        public PrediccionService(IUnitOfWork unitOfWork, IPagoService pagoService)
        {
            _unitOfWork = unitOfWork;
            _pagoService = pagoService;
        }

        public async Task<PrediccionResponseDto> CrearOModificarAsync(PrediccionRequestDto dto, Guid usuarioId)
        {
            // 1. Verificar que el usuario pagó en la penca
            var pago = await _pagoService.UsuarioPagoEnPencaAsync(usuarioId, dto.PencaId);
            if (!pago)
                throw new Exception("Debes estar inscripto en la penca para predecir");

            // 2. Verificar que el partido existe y pertenece al evento de la penca
            var partido = await _unitOfWork.Partidos.GetByIdAsync(dto.PartidoId);
            if (partido == null)
                throw new Exception("Partido no encontrado");

            // 3. Verificar que el partido no empezó todavía
            var penca = await _unitOfWork.Pencas.GetByIdConDetalleAsync(dto.PencaId);
            if (penca == null)
                throw new Exception("Penca no encontrada");

            var tiempoLimite = partido.Fecha.AddMinutes(-penca.Plantilla.TiempoLimitePrevioMinutos);
            if (DateTime.UtcNow >= tiempoLimite)
                throw new Exception("El tiempo límite para predecir este partido ya cerró");

            // 4. Verificar si ya existe una predicción para este partido en esta penca
            var predicciones = await _unitOfWork.Predicciones.GetAllAsync();
            var prediccionExistente = predicciones.FirstOrDefault(p =>
                p.UsuarioId == usuarioId &&
                p.PartidoId == dto.PartidoId &&
                p.PencaId == dto.PencaId);

            Guid prediccionId;

            if (prediccionExistente != null)
            {
                prediccionId = prediccionExistente.Id; // ← guardamos el Id existente
                prediccionExistente.GolesLocal = dto.GolesLocal;
                prediccionExistente.GolesVisitante = dto.GolesVisitante;
                await _unitOfWork.Predicciones.UpdateAsync(prediccionExistente);
            }
            else
            {
                prediccionId = Guid.NewGuid(); // ← generamos el Id nuevo
                var prediccion = new Prediccion
                {
                    Id = prediccionId,
                    UsuarioId = usuarioId,
                    PartidoId = dto.PartidoId,
                    PencaId = dto.PencaId,
                    GolesLocal = dto.GolesLocal,
                    GolesVisitante = dto.GolesVisitante
                };
                await _unitOfWork.Predicciones.AddAsync(prediccion);
            }

            await _unitOfWork.SaveChangesAsync();

            // Cargar equipos para el response
            var equipoLocal = await _unitOfWork.Equipos.GetByIdAsync(partido.EquipoLocalId);
            var equipoVisitante = await _unitOfWork.Equipos.GetByIdAsync(partido.EquipoVisitanteId);

            return new PrediccionResponseDto
            {
                Id = prediccionId, // ← siempre correcto
                PartidoId = dto.PartidoId,
                EquipoLocal = equipoLocal?.Nombre ?? string.Empty,
                EquipoVisitante = equipoVisitante?.Nombre ?? string.Empty,
                GolesLocal = dto.GolesLocal,
                GolesVisitante = dto.GolesVisitante,
                FechaPartido = partido.Fecha
            };
        }

        public async Task<IEnumerable<PrediccionResponseDto>> ObtenerMisPrediccionesAsync(Guid usuarioId, Guid pencaId)
        {
            var predicciones = await _unitOfWork.Predicciones.GetAllAsync();
            var misPredicciones = predicciones
                .Where(p => p.UsuarioId == usuarioId && p.PencaId == pencaId)
                .ToList();

            var resultado = new List<PrediccionResponseDto>();
            foreach (var pred in misPredicciones)
            {
                var partido = await _unitOfWork.Partidos.GetByIdAsync(pred.PartidoId);
                var equipoLocal = await _unitOfWork.Equipos.GetByIdAsync(partido!.EquipoLocalId);
                var equipoVisitante = await _unitOfWork.Equipos.GetByIdAsync(partido.EquipoVisitanteId);

                resultado.Add(new PrediccionResponseDto
                {
                    Id = pred.Id,
                    PartidoId = pred.PartidoId,
                    EquipoLocal = equipoLocal?.Nombre ?? string.Empty,
                    EquipoVisitante = equipoVisitante?.Nombre ?? string.Empty,
                    GolesLocal = pred.GolesLocal,
                    GolesVisitante = pred.GolesVisitante,
                    FechaPartido = partido.Fecha
                });
            }

            return resultado;
        }
    }
}
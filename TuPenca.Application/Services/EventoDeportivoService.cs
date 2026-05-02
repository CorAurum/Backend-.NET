using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Application.DTOs.EventoDeportivo;
using TuPenca.Application.DTOs.Partido;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class EventoDeportivoService : IEventoDeportivoService
    {
        private readonly IUnitOfWork _unitOfWork;


        public EventoDeportivoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EventoDeportivoResponseDto>> ObtenerTodosAsync()
        {
            var eventos = await _unitOfWork.EventosDeportivos.GetAllAsync();
            return eventos.Select(e => new EventoDeportivoResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                FechaInicio = e.FechaInicio,
                FechaFin = e.FechaFin
            });
        }

        public async Task<EventoDeportivoResponseDto?> ObtenerPorIdAsync(Guid id)
        {
            var evento = await _unitOfWork.EventosDeportivos.GetByIdAsync(id);
            if (evento == null) return null;

            return new EventoDeportivoResponseDto
            {
                Id = evento.Id,
                Nombre = evento.Nombre,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin
            };
        }

        public async Task<EventoDeportivoResponseDto> CrearAsync(EventoDeportivoRequestDto dto)
        {
            var evento = new EventoDeportivo
            {
                Id = Guid.NewGuid(),
                Nombre = dto.Nombre,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                DeporteId = dto.DeporteId,
                TipoCompetenciaId = dto.TipoCompetenciaId
            };

            await _unitOfWork.EventosDeportivos.AddAsync(evento);
            await _unitOfWork.SaveChangesAsync();

            return new EventoDeportivoResponseDto
            {
                Id = evento.Id,
                Nombre = evento.Nombre,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin
            };
        }

        public async Task<PartidoResponseDto> AgregarPartidoAsync(PartidoRequestDto dto)
        {
            var evento = await _unitOfWork.EventosDeportivos.GetByIdAsync(dto.EventoDeportivoId);
            if (evento == null)
                throw new Exception("Evento deportivo no encontrado");

            var equipoLocal = await _unitOfWork.Equipos.GetByIdAsync(dto.EquipoLocalId);
            var equipoVisitante = await _unitOfWork.Equipos.GetByIdAsync(dto.EquipoVisitanteId);

            if (equipoLocal == null || equipoVisitante == null)
                throw new Exception("Uno o ambos equipos no encontrados");

            if (dto.EquipoLocalId == dto.EquipoVisitanteId)
                throw new Exception("El equipo local y visitante no pueden ser el mismo");

            var partido = new Partido
            {
                Id = Guid.NewGuid(),
                Fecha = dto.Fecha,
                Fase = dto.Fase,
                EquipoLocalId = dto.EquipoLocalId,
                EquipoVisitanteId = dto.EquipoVisitanteId,
                EventoDeportivoId = dto.EventoDeportivoId
            };

            await _unitOfWork.Partidos.AddAsync(partido);
            await _unitOfWork.SaveChangesAsync();

            return new PartidoResponseDto
            {
                Id = partido.Id,
                Fecha = partido.Fecha,
                Fase = partido.Fase,
                EquipoLocal = equipoLocal.Nombre,
                EquipoVisitante = equipoVisitante.Nombre
            };
        }


        public async Task<ResultadoResponseDto> CargarResultadoAsync(ResultadoRequestDto dto)
        {
            // 1. Verificar que el partido existe
            var partido = await _unitOfWork.Partidos.GetByIdAsync(dto.PartidoId);
            if (partido == null)
                throw new Exception("Partido no encontrado");

            // 2. Cargar resultado
            partido.ResultadoLocal = dto.GolesLocal;
            partido.ResultadoVisitante = dto.GolesVisitante;
            await _unitOfWork.Partidos.UpdateAsync(partido);

            // 3. Traer todas las predicciones de este partido con detalle
            var predicciones = await _unitOfWork.Predicciones.GetByPartidoConDetalleAsync(dto.PartidoId);

            int usuariosActualizados = 0;

            foreach (var prediccion in predicciones)
            {
                // 4. Calcular desviacion
                var desviacion = Math.Abs(dto.GolesLocal - prediccion.GolesLocal)
                               + Math.Abs(dto.GolesVisitante - prediccion.GolesVisitante);

                // 5. Buscar puntaje en reglas de la plantilla de esa penca
                var reglas = prediccion.Penca.Plantilla.Reglas
                    .OrderBy(r => r.Desviacion)
                    .ToList();

                var regla = reglas.FirstOrDefault(r => r.Desviacion == desviacion);
                var puntos = regla?.Puntaje ?? 0;

                // 6. Buscar si ya existe un PuntajeUsuario para este usuario/penca/partido
                var puntajeExistente = await _unitOfWork.PuntajesUsuario
                    .GetByUsuarioPencaPartidoAsync(prediccion.UsuarioId, prediccion.PencaId, dto.PartidoId);

                if (puntajeExistente != null)
                {
                    puntajeExistente.PuntosPartido = puntos;
                    await _unitOfWork.PuntajesUsuario.UpdateAsync(puntajeExistente);
                }
                else
                {
                    var nuevoPuntaje = new PuntajeUsuario
                    {
                        Id = Guid.NewGuid(),
                        UsuarioId = prediccion.UsuarioId,
                        PencaId = prediccion.PencaId,
                        PartidoId = dto.PartidoId,
                        PuntosPartido = puntos
                    };
                    await _unitOfWork.PuntajesUsuario.AddAsync(nuevoPuntaje);
                }

                usuariosActualizados++;
            }

            await _unitOfWork.SaveChangesAsync();

            var equipoLocal = await _unitOfWork.Equipos.GetByIdAsync(partido.EquipoLocalId);
            var equipoVisitante = await _unitOfWork.Equipos.GetByIdAsync(partido.EquipoVisitanteId);

            return new ResultadoResponseDto
            {
                PartidoId = partido.Id,
                EquipoLocal = equipoLocal?.Nombre ?? string.Empty,
                EquipoVisitante = equipoVisitante?.Nombre ?? string.Empty,
                GolesLocal = dto.GolesLocal,
                GolesVisitante = dto.GolesVisitante,
                UsuariosActualizados = usuariosActualizados
            };
        }

    }
}
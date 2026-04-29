using TuPenca.Application.DTOs.PlantillaPenca;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class PlantillaPencaService : IPlantillaPencaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlantillaPencaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlantillaPencaResponseDto>> ObtenerTodasAsync()
        {
            var plantillas = await _unitOfWork.PlantillasPenca.GetAllConDetalleAsync();
            return plantillas.Select(p => new PlantillaPencaResponseDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                TiempoLimitePrevioMinutos = p.TiempoLimitePrevioMinutos,
                EventoDeportivo = p.Evento?.Nombre ?? string.Empty,
                Reglas = p.Reglas.Select(r => new ReglaPuntuacionDto
                {
                    Desviacion = r.Desviacion,
                    Puntaje = r.Puntaje
                }).ToList()
            });
        }

        public async Task<PlantillaPencaResponseDto?> ObtenerPorIdAsync(Guid id)
        {
            var plantilla = await _unitOfWork.PlantillasPenca.GetByIdAsync(id);
            if (plantilla == null) return null;

            return new PlantillaPencaResponseDto
            {
                Id = plantilla.Id,
                Nombre = plantilla.Nombre,
                Descripcion = plantilla.Descripcion,
                TiempoLimitePrevioMinutos = plantilla.TiempoLimitePrevioMinutos,
                EventoDeportivo = plantilla.Evento?.Nombre ?? string.Empty,
                Reglas = plantilla.Reglas.Select(r => new ReglaPuntuacionDto
                {
                    Desviacion = r.Desviacion,
                    Puntaje = r.Puntaje
                }).ToList()
            };
        }

        public async Task<PlantillaPencaResponseDto> CrearAsync(PlantillaPencaRequestDto dto)
        {
            var evento = await _unitOfWork.EventosDeportivos.GetByIdAsync(dto.EventoDeportivoId);
            if (evento == null)
                throw new Exception("Evento deportivo no encontrado");

            if (!dto.Reglas.Any())
                throw new Exception("La plantilla debe tener al menos una regla de puntuación");

            if (dto.Reglas.Any(r => r.Puntaje < 0 || r.Desviacion < 0))
                throw new Exception("Desviación y puntaje deben ser valores positivos");

            var plantilla = new PlantillaPenca
            {
                Id = Guid.NewGuid(),
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                TiempoLimitePrevioMinutos = dto.TiempoLimitePrevioMinutos,
                EventoDeportivoId = dto.EventoDeportivoId,
                Reglas = dto.Reglas.Select(r => new ReglaPuntuacion
                {
                    Id = Guid.NewGuid(),
                    Desviacion = r.Desviacion,
                    Puntaje = r.Puntaje
                }).ToList()
            };

            await _unitOfWork.PlantillasPenca.AddAsync(plantilla);
            await _unitOfWork.SaveChangesAsync();

            return new PlantillaPencaResponseDto
            {
                Id = plantilla.Id,
                Nombre = plantilla.Nombre,
                Descripcion = plantilla.Descripcion,
                TiempoLimitePrevioMinutos = plantilla.TiempoLimitePrevioMinutos,
                EventoDeportivo = evento.Nombre,
                Reglas = dto.Reglas
            };
        }

        public async Task EliminarAsync(Guid id)
        {
            var plantilla = await _unitOfWork.PlantillasPenca.GetByIdAsync(id);
            if (plantilla == null)
                throw new Exception("Plantilla no encontrada");

            if (plantilla.Pencas.Any())
                throw new Exception("No se puede eliminar una plantilla que tiene pencas instanciadas");

            await _unitOfWork.PlantillasPenca.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
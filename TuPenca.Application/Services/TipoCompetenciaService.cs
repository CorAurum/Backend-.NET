using TuPenca.Application.DTOs.TipoCompetencia;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class TipoCompetenciaService : ITipoCompetenciaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoCompetenciaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TipoCompetenciaResponseDto>> ObtenerTodosAsync()
        {
            var competencias = await _unitOfWork.TiposCompetencias.GetAllAsync();
            return competencias.Select(e => new TipoCompetenciaResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre
            });
        }

        public async Task<IEnumerable<TipoCompetenciaResponseDto>> CrearVariosAsync(TipoCompetenciaRequestDto dto)
        {
            if (!dto.Nombres.Any())
                throw new Exception("Debe enviar al menos un equipo");

            var competencias = dto.Nombres.Select(nombre => new TipoCompetencia
            {
                Id = Guid.NewGuid(),
                Nombre = nombre
            }).ToList();

            foreach (var competencia in competencias)
                await _unitOfWork.TiposCompetencias.AddAsync(competencia);

            await _unitOfWork.SaveChangesAsync();

            return competencias.Select(e => new TipoCompetenciaResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre
            });
        }

    }
}

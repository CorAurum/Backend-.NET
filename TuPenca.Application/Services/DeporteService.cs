using TuPenca.Application.DTOs.Deporte;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class DeporteService : IDeporteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeporteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DeporteResponseDto>> ObtenerTodosAsync()
        {
            var equipos = await _unitOfWork.Deportes.GetAllAsync();
            return equipos.Select(e => new DeporteResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre
            });
        }

        public async Task<IEnumerable<DeporteResponseDto>> CrearVariosAsync(DeporteRequestDto dto)
        {
            if (!dto.Nombres.Any())
                throw new Exception("Debe enviar al menos un deporte");

            var deportes = dto.Nombres.Select(nombre => new Deporte
            {
                Id = Guid.NewGuid(),
                Nombre = nombre
            }).ToList();

            foreach (var deporte in deportes)
                await _unitOfWork.Deportes.AddAsync(deporte);

            await _unitOfWork.SaveChangesAsync();

            return deportes.Select(e => new DeporteResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre
            });
        }
    }
}

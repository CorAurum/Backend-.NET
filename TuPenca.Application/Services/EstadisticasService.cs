using TuPenca.Application.DTOs.Estadisticas;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Enums;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class EstadisticasService : IEstadisticasService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EstadisticasService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EstadisticasGlobalesDto> ObtenerGlobalesAsync()
        {
            var sitios = await _unitOfWork.Sitios.GetAllAsync();
            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            var pencas = await _unitOfWork.Pencas.GetAllConDetalleAsync();
            var pagos = await _unitOfWork.Pagos.GetAllAsync();

            var pagosAprobados = pagos.Where(p => p.Estado == EstadoPago.Aprobado).ToList();

            var totalRecaudado = 0;
            var totalComisiones = 0;

            foreach (var pago in pagosAprobados)
            {
                var penca = pencas.FirstOrDefault(p => p.Id == pago.PencaId);
                if (penca?.Plantilla == null) continue;

                totalRecaudado += pago.Monto;
                totalComisiones += pago.Monto * penca.Plantilla.PorcentajeComision / 100;
            }

            // Top 5 sitios por usuarios
            var topPorUsuarios = usuarios
                .GroupBy(u => u.SitioId)
                .Select(g => new
                {
                    SitioId = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .Select(x => new EstadisticaSitioResumenDto
                {
                    NombreSitio = sitios.FirstOrDefault(s => s.Id == x.SitioId)?.Nombre ?? string.Empty,
                    Valor = x.Total
                })
                .ToList();

            // Top 5 sitios por recaudacion
            var topPorRecaudacion = pagosAprobados
                .Join(pencas, p => p.PencaId, penca => penca.Id, (p, penca) => new { p.Monto, penca.SitioId })
                .GroupBy(x => x.SitioId)
                .Select(g => new
                {
                    SitioId = g.Key,
                    Total = g.Sum(x => x.Monto)
                })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .Select(x => new EstadisticaSitioResumenDto
                {
                    NombreSitio = sitios.FirstOrDefault(s => s.Id == x.SitioId)?.Nombre ?? string.Empty,
                    Valor = x.Total
                })
                .ToList();

            return new EstadisticasGlobalesDto
            {
                TotalSitios = sitios.Count(),
                TotalUsuarios = usuarios.Count(),
                TotalPencasActivas = pencas.Count(p => p.Estado == EstadoPenca.Abierta || p.Estado == EstadoPenca.EnCurso),
                TotalPencasFinalizadas = pencas.Count(p => p.Estado == EstadoPenca.Finalizada),
                TotalRecaudado = totalRecaudado,
                TotalComisionesGeneradas = totalComisiones,
                TopSitiosPorUsuarios = topPorUsuarios,
                TopSitiosPorRecaudacion = topPorRecaudacion
            };
        }

        public async Task<EstadisticasSitioDto> ObtenerPorSitioAsync(Guid sitioId)
        {
            var sitio = await _unitOfWork.Sitios.GetByIdAsync(sitioId);
            if (sitio == null)
                throw new Exception("Sitio no encontrado");

            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            var usuariosSitio = usuarios.Where(u => u.SitioId == sitioId).ToList();

            var pencas = await _unitOfWork.Pencas.GetAllConDetalleAsync();
            var pencasSitio = pencas.Where(p => p.SitioId == sitioId).ToList();

            var pagos = await _unitOfWork.Pagos.GetAllAsync();
            var pagosSitio = pagos
                .Where(p => p.Estado == EstadoPago.Aprobado &&
                            pencasSitio.Any(penca => penca.Id == p.PencaId))
                .ToList();

            var totalRecaudadoSitio = 0;
            var totalComisionesSitio = 0;

            foreach (var pago in pagosSitio)
            {
                var penca = pencasSitio.FirstOrDefault(p => p.Id == pago.PencaId);
                if (penca?.Plantilla == null) continue;

                totalRecaudadoSitio += pago.Monto;
                totalComisionesSitio += pago.Monto * penca.Plantilla.PorcentajeComision / 100;
            }

            var estadisticasPorPenca = new List<EstadisticaPencaDto>();

            foreach (var penca in pencasSitio)
            {
                var participantes = pagosSitio.Count(p => p.PencaId == penca.Id);

                var puntajes = await _unitOfWork.PuntajesUsuario.GetByPencaAsync(penca.Id);
                var lider = puntajes
                    .GroupBy(p => new { p.UsuarioId, p.Usuario.Nombre })
                    .Select(g => new
                    {
                        Nombre = g.Key.Nombre,
                        Puntos = g.Sum(p => p.PuntosPartido)
                    })
                    .OrderByDescending(x => x.Puntos)
                    .FirstOrDefault();

                var predicciones = await _unitOfWork.Predicciones.GetAllAsync();
                var partidosConPrediccion = predicciones
                    .Where(p => p.PencaId == penca.Id)
                    .Select(p => p.PartidoId)
                    .Distinct()
                    .Count();

                estadisticasPorPenca.Add(new EstadisticaPencaDto
                {
                    NombrePenca = penca.Nombre,
                    LiderActual = lider?.Nombre ?? "Sin predicciones",
                    PuntosLider = lider?.Puntos ?? 0,
                    TotalParticipantes = participantes,
                    TotalPartidosConPrediccion = partidosConPrediccion
                });
            }

            return new EstadisticasSitioDto
            {
                NombreSitio = sitio.Nombre,
                TotalUsuarios = usuariosSitio.Count,
                TotalPencasActivas = pencasSitio.Count(p => p.Estado == EstadoPenca.Abierta || p.Estado == EstadoPenca.EnCurso),
                TotalPencasFinalizadas = pencasSitio.Count(p => p.Estado == EstadoPenca.Finalizada),
                TotalRecaudado = totalRecaudadoSitio,
                TotalComisionesGeneradas = totalComisionesSitio,
                EstadisticasPorPenca = estadisticasPorPenca
            };
        }
    }
}
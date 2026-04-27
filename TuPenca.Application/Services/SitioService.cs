using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Application.DTOs.Sitio;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;

namespace TuPenca.Application.Services
{
    public class SitioService : ISitioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SitioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SitioResponseDto>> ObtenerSitiosAsync()
        {
            var sitios = await _unitOfWork.Sitios.GetAllAsync();
            var result = new List<SitioResponseDto>();
            foreach (var sitio in sitios)
            {
                result.Add(new SitioResponseDto()
                {
                    Id = sitio.Id,
                    Nombre = sitio.Nombre,
                    UrlPropia = sitio.UrlPropia,
                    ConfiguracionSitio = sitio.ConfiguracionSitio,
                    EsquemaColores = sitio.EsquemaColores,
                    TipoRegistro = sitio.TipoRegistro
                });
            }
            return result;
        }

        public async Task<SitioResponseDto> ObtenerSitioAsync(Guid sitioId)
        {
            var sitio = await _unitOfWork.Sitios.GetByIdAsync(sitioId);
            return new SitioResponseDto()
            {
                Id = sitio.Id,
                Nombre = sitio.Nombre,
                UrlPropia = sitio.UrlPropia,
                ConfiguracionSitio = sitio.ConfiguracionSitio,
                EsquemaColores = sitio.EsquemaColores,
                TipoRegistro = sitio.TipoRegistro
            };
        }

        public async Task<SitioResponseDto> CrearSitioAsync(SitioRequestDto sitioDto)
        {
            sitioDto.Id = Guid.NewGuid();

            var sitio = new Sitio()
            {
                Id = sitioDto.Id,
                Nombre = sitioDto.Nombre,
                UrlPropia = sitioDto.UrlPropia,
                ConfiguracionSitio = sitioDto.ConfiguracionSitio,
                EsquemaColores = sitioDto.EsquemaColores,
                TipoRegistro = sitioDto.TipoRegistro
            };

            await _unitOfWork.Sitios.AddAsync(sitio);
            await _unitOfWork.SaveChangesAsync();

            return new SitioResponseDto()
            {
                Id = sitio.Id,
                Nombre = sitio.Nombre,
                UrlPropia = sitio.UrlPropia,
                ConfiguracionSitio = sitio.ConfiguracionSitio,
                EsquemaColores = sitio.EsquemaColores,
                TipoRegistro = sitio.TipoRegistro
            };
        }

        //public async Task<Tenant> ActualizarSitio(Guid id, Tenant updated)
        //{
        //    var tenant = await _context.Tenants.FindAsync(id);

        //    if (tenant == null)
        //        throw new Exception("Tenant no encontrado");

        //    tenant.Name = updated.Name;
        //    tenant.Domain = updated.Domain;
        //    tenant.PrimaryColor = updated.PrimaryColor;
        //    tenant.SecondaryColor = updated.SecondaryColor;

        //    await _context.SaveChangesAsync();

        //    return tenant;
        //}

        //public async Task EliminarSitio(Guid id)
        //{
        //    var tenant = await _context.Tenants.FindAsync(id);

        //    if (tenant == null)
        //        throw new Exception("Tenant no encontrado");

        //    _context.Tenants.Remove(tenant);
        //    await _context.SaveChangesAsync();
        //}
    }
}

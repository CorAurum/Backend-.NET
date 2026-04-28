using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TuPenca.Domain.Entities;
using TuPenca.Infrastructure.Interfaces.Providers;

namespace TuPenca.Infrastructure.Providers
{
    public class SitioProvider : ISitioProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SitioProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetSitioId()
        {
            // JWT
            var claim = _httpContextAccessor.HttpContext?
                .User?.FindFirst("sitioId")?.Value;

            if (claim != null)
                return Guid.Parse(claim);

            // Dominio
            var sitio = _httpContextAccessor.HttpContext?
                .Items["Sitio"] as Sitio;

            return sitio?.Id;
        }

        public bool EsAdminPlataforma()
        {
            return _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Role)?.Value == "AdministradorPlataforma";
        }

    }
}

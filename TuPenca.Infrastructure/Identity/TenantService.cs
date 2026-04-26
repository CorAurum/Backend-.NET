using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TuPenca.Application.Interfaces.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetSitioId()
    {
        var claim = _httpContextAccessor.HttpContext?.User
            .FindFirst("sitioId")?.Value;

        if (string.IsNullOrEmpty(claim))
            return null;

        return Guid.TryParse(claim, out var sitioId) ? sitioId : null;
    }

    public bool EsAdminPlataforma()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Role)?.Value == "AdministradorPlataforma";
    }
}
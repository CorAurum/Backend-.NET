using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.Interfaces.Services
{
    public interface ITenantService
    {
        Guid? GetSitioId();         // null si es admin de plataforma
        bool EsAdminPlataforma();   // true si el rol es AdministradorPlataforma
    }
}

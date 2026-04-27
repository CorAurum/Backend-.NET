using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Infrastructure.Interfaces.Providers
{
    public interface ISitioProvider
    {
        Guid? GetSitioId();         // null si es admin de plataforma
        bool EsAdminPlataforma();   // true si el rol es AdministradorPlataforma
    }
}

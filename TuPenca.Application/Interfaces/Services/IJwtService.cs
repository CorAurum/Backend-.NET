using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerarToken(string id, string email, string nombre, string rol);
    }
}

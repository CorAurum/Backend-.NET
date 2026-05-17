using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.DTOs.Auth
{
    public class FirebaseLoginRequest
    {
        public string IdToken { get; set; } = null!;
    }
}

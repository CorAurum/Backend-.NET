using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Application.Interfaces.Services
{
    public interface IFirebaseService
    {
        Task<FirebaseToken> VerifyTokenAsync(string idToken);
    }
}

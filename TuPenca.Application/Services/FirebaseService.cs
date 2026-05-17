using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using TuPenca.Application.Interfaces.Services;

namespace TuPenca.Application.Services
{
    public class FirebaseService : IFirebaseService
    {
        public async Task<FirebaseToken> VerifyTokenAsync(string idToken)
        {
            return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        }
    }
}

using System;
using AspNetBearerAuthentication.Models;

namespace AspNetBearerAuthentication.Repository
{
    public sealed class UserMocRepository : IUserRepository
    {
        private const string Token = "auth-token";

        public UserModel GetUser(string token)
        {
            if (string.IsNullOrEmpty(token) || !string.Equals(token,Token,StringComparison.Ordinal)) 
                return null;
            
            return new UserModel
            {
                UserId = 1000,
                FirstName = "Deepu",
                LastName = "Madhusoodanan"
            };
        }
    }
}
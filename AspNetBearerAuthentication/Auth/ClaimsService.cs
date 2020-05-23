using System.Security.Claims;
using AspNetBearerAuthentication.Models;

namespace AspNetBearerAuthentication.Auth
{
    public sealed class ClaimsService : ICalimsService
    {
        public ClaimsPrincipal CreateClaimsPrincipal(UserModel user)
        {
            if (user == null || user.UserId == default) return default;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            var identity = new ClaimsIdentity(claims, "Bearer");

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
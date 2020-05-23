using System.Security.Claims;
using AspNetBearerAuthentication.Models;

namespace AspNetBearerAuthentication.Auth
{
    public interface ICalimsService
    {
        ClaimsPrincipal CreateClaimsPrincipal(UserModel model);
    }
}
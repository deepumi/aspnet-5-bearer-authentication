using AspNetBearerAuthentication.Models;

namespace AspNetBearerAuthentication.Repository
{
    public interface IUserRepository
    {
        UserModel GetUser(string token);
    }
}
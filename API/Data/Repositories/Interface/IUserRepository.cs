using API.Entities;

namespace API.Data.Repositories.Interface
{
    public interface IUserRepository
    {
        User GetUserByToken(string token);
        User UpdateUser(User user);
    }
}

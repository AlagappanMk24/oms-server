using API.Data.Context;
using API.Data.Repositories.Interface;
using API.Entities;

namespace API.Data.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public User GetUserByToken(string token)
        {
            return _dbContext.Users.FirstOrDefault(u => u.ResetToken == token);
        }
        public User UpdateUser(User user)
        {
            _dbContext.SaveChanges();
            return user; // return updated user
        }
    }
}

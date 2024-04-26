using Microsoft.EntityFrameworkCore;
using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository(DbSet<User> users)
        {
            using var context = new AppDBContext();
            var _users = new List<User>
            {
                new User { Id = 1, Email = "test@mail.ru", passHash = "1234"},
                new User { Id = 2, Email = "admin@mail.ru", passHash = "admin"}
            };
            context.Users.AddRange(_users);
            context.SaveChanges();
        }
        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id) => _users.FirstOrDefault(x => x.Id == id);

        public IEnumerable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(int id, User newUser)
        {
            throw new NotImplementedException();
        }
    }
}

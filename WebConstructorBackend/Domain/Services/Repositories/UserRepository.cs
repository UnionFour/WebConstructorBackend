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
                new User { ID = Guid.Parse("80270d58-6165-4cf1-b910-72b969bff3f5"), Email = "test@mail.ru", passHash = "1234"},
                new User { ID = Guid.Parse("0f6022df-4af4-42ca-b513-15a7be3a52fd"), Email = "admin@mail.ru", passHash = "admin"}
            };
            context.Users.AddRange(_users);
            context.SaveChanges();
        }
        public void DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(Guid id) => _users.FirstOrDefault(x => x.ID == id);

        public IEnumerable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(Guid id, User newUser)
        {
            throw new NotImplementedException();
        }
    }
}

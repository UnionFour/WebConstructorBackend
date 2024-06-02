using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IUserRepository
    {
        public User GetUser(Guid id);

        public User CreateUser(User user);

        public User GetUserByEmail(string email);

        public IEnumerable<User> GetUsers();

        public User UpdateUser(User newUser);

        public void DeleteUser(Guid id);
    }
}

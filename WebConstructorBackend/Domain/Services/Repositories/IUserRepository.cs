using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IUserRepository
    {
        public User GetUser(int id);

        public IEnumerable<User> GetUsers();

        public User UpdateUser(int id, User newUser);

        public void DeleteUser(int id);
    }
}

using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IUserRepository
    {
        public User GetUser(Guid id);

        public IEnumerable<User> GetUsers();

        public User UpdateUser(Guid id, User newUser);

        public void DeleteUser(Guid id);
    }
}

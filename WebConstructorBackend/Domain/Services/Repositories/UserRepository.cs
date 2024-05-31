using Microsoft.EntityFrameworkCore;
using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _db;
        public UserRepository([Service] AppDBContext db)
        {
            _db = db;
        }

        public void DeleteUser(Guid id)
        {
            var user = _db.Users.FirstOrDefault(x => x.ID == id);
            if(user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
            }
        }

        public User GetUser(Guid id) 
        {
            return _db.Users.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<User> GetUsers()
        {
            return _db.Users;
        }

        public User UpdateUser(User newUser)
        {
            if (newUser != null)
            {
                _db.Users.Update(newUser);
                _db.SaveChanges();
            }
            return newUser;
        }
    }
}

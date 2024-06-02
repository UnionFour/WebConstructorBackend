using Microsoft.AspNetCore.Mvc;
using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _db;
        public UserRepository([FromServices] AppDBContext db)
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

        public User CreateUser(User user)
        {
            if (user.ID == Guid.Empty)
                user.ID = Guid.NewGuid();

            _db.Users.Add(user);
            _db.SaveChanges();

            return user;
        }

        public User GetUser(Guid id) 
        {
            return _db.Users.FirstOrDefault(x => x.ID == id);
        }

        public User GetUserByEmail(string email) 
        {
            return _db.Users.FirstOrDefault(x => x.Email == email);
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

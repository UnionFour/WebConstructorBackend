using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class GymRepository : IGymRepository
    {
        private readonly AppDBContext _db;
        public GymRepository([Service] AppDBContext db) 
        {
            _db = db;
        }

        public Gym CreateGym(Gym gym)
        {
            throw new NotImplementedException();
        }

        public Gym DeleteGym(Guid id)
        {
            throw new NotImplementedException();
        }

        public Gym GetGymById(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Gym> GetOrganizationsGyms(Guid organizationID)
        {
            throw new NotImplementedException();
        }

        public Gym UpdateGym(Guid id, Gym gym)
        {
            throw new NotImplementedException();
        }
    }
}

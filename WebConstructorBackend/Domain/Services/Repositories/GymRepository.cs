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
            if (gym.ID == Guid.Empty)
                gym.ID = Guid.NewGuid();

            _db.Gyms.Add(gym);
            _db.SaveChanges();

            return gym;
        }

        public void DeleteGym(Guid id)
        {
            var gym = _db.Gyms.FirstOrDefault(x => x.ID == id);

            _db.Gyms.Remove(gym);
            _db.SaveChanges();

        }

        public Gym GetGymById(Guid id)
        {
            return _db.Gyms.FirstOrDefault(x => x.ID == id);
        }

        public List<Gym> GetOrganizationsGyms(Guid organizationID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);

            if (org == null)
                return null;
            return org.Gyms;
        }

        public Gym UpdateGym(Gym gym)
        {
            _db.Gyms.Update(gym);
            _db.SaveChanges();

            return gym;
        }
    }
}

using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDBContext _db;
        public OrganizationRepository([Service] AppDBContext db)
        {
            _db = db;
        }

        public User AddCouch(Guid organizationID, User couch)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;
            
            org.Couches.Add(couch);
            _db.Organizations.Update(org);
            _db.SaveChanges();

            return couch;
        }

        public User GetCouch(Guid organizationID, Guid couchID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return org.Couches.FirstOrDefault(x => x.ID == couchID);
        }

        public List<User> GetCouches(Guid organizationID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return org.Couches;
        }

        public Gym GetGym(Guid organizationID, Guid gymID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return org.Gyms.FirstOrDefault(x => x.ID == gymID);
        }

        public List<Gym> GetGymes(Guid organizationID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return org.Gyms;
        }

        public Organization GetOrganization(Guid id)
        {
            return _db.Organizations.FirstOrDefault(o => o.ID == id);
        }

        public User GetOrganizator(Guid organizationID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return _db.Users.FirstOrDefault(x => x.ID == org.OrganizatorID);
        }

        public List<User> GetVisitors(Guid organizationID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return null;
        }
    }
}

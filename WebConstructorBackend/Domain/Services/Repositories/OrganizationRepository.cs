using Microsoft.AspNetCore.Mvc;
using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDBContext _db;
        public OrganizationRepository([FromServices] AppDBContext db)
        {
            _db = db;
        }

        public Organization CreateOrganization(Organization organization)
        {
            if (organization.ID == Guid.Empty)
                organization.ID = Guid.NewGuid();

            if (_db.Organizations.Where(x => x.Name == organization.Name) == null)
            {
                _db.Organizations.Add(organization);
                _db.SaveChanges();
            }

            return organization;
        }

        public Organization UpdateOrganization(Organization organization)
        {
            if (organization != null)
                _db.Organizations.Update(organization);

            return organization;
        }

        public Organization GetOrganizationByName(string name)
        {
            return _db.Organizations.FirstOrDefault(x => x.Name == name);
        }

        public Couch AddCouch(Guid organizationID, Couch couch)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;
            
            org.Couches.Add(couch);
            _db.Organizations.Update(org);
            _db.SaveChanges();

            return couch;
        }

        public void RemoveCouch(Guid organizationID, Couch couch)
        {
            var org = GetOrganization(organizationID);
            org.Couches.Remove(couch);
        }

        public User GetCouch(Guid organizationID, Guid couchID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return org.Couches.FirstOrDefault(x => x.ID == couchID);
        }

        public List<Couch> GetCouches(Guid organizationID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return org.Couches.ToList();
        }

        public Gym GetGym(Guid organizationID, Guid gymID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return org.Gyms.FirstOrDefault(x => x.ID == gymID);
        }

        public Gym AddGym(Guid organizationID, Gym gym)
        {
            var org = GetOrganization(organizationID);
            org.Gyms.Add(gym);
            return gym;
        }

        public void RemoveGym(Guid organizationID, Gym gym)
        {
            var org = GetOrganization(organizationID);
            org.Gyms.Remove(gym);
        }

        public List<Gym> GetGymes(Guid organizationID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return org.Gyms.ToList();
        }

        public Organization GetOrganization(Guid id)
        {
            return _db.Organizations.FirstOrDefault(o => o.ID == id);
        }

        public Organizator GetOrganizator(Guid organizationID)
        {
            var org = _db.Organizations.FirstOrDefault(x => x.ID == organizationID);
            if (org == null)
                return null;

            return (Organizator)_db.Users.FirstOrDefault(x => x.ID == org.OrganizatorID);
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

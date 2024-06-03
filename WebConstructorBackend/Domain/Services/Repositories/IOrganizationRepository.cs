using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IOrganizationRepository
    {
        public Organization GetOrganization(Guid id);

        public Organization CreateOrganization(Organization organization);

        public Organization GetOrganizationByName(string name);

        public Organization UpdateOrganization(Organization organization);

        public List<Couch> GetCouches(Guid organizationID);

        public User GetCouch(Guid organizationID, Guid couchID);

        public Organizator GetOrganizator(Guid organizationID);

        public Gym AddGym(Guid organizationID, Gym gym);

        public void RemoveGym(Guid organizationID, Gym gym);

        public List<Gym> GetGymes(Guid organizationID);

        public Gym GetGym(Guid organizationID, Guid gymID);

        public Couch AddCouch(Guid organizationID, Couch couch);

        public void RemoveCouch(Guid organizationID, Couch couch);

        public List<User> GetVisitors(Guid organizationID);
    }
}

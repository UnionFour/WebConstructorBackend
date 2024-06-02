using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IOrganizationRepository
    {
        public Organization GetOrganization(Guid id);

        public List<Couch> GetCouches(Guid organizationID);

        public User GetCouch(Guid organizationID, Guid couchID);

        public Organizator GetOrganizator(Guid organizationID);

        public List<Gym> GetGymes(Guid organizationID);

        public Gym GetGym(Guid organizationID, Guid gymID);

        public Couch AddCouch(Guid organizationID, Couch couch);

        public List<User> GetVisitors(Guid organizationID);
    }
}

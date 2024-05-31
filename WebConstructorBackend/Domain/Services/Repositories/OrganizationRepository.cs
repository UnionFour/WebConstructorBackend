using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly List<Organization> _organizations;

        public OrganizationRepository()
        {

        }

        public User AddCouch(User couch)
        {

        }

        public User GetCouch(Guid couchID)
        {
            throw new NotImplementedException();
        }

        public List<User> GetCouches(Guid organizationID)
        {
            throw new NotImplementedException();
        }

        public Gym GetGym(Guid gymID)
        {
            throw new NotImplementedException();
        }

        public List<Gym> GetGymes(Guid organizationID)
        {
            throw new NotImplementedException();
        }

        public Organization GetOrganization(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetOrganizator(Guid organizationID)
        {
            throw new NotImplementedException();
        }

        public List<User> GetVisitors(Guid organizationID)
        {
            throw new NotImplementedException();
        }
    }
}

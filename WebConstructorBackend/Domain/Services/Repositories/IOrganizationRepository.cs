using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IOrganizationRepository
    {
        public Organization GetOrganization(Guid id);

        public List<User> GetCouches(Guid organizationID);

        public User GetCouch(Guid couchID);

        public User GetOrganizator(Guid organizationID);

        public List<Gym> GetGymes(Guid organizationID);

        public Gym GetGym(Guid gymID);

        public User AddCouch(User couch);

        public List<User> GetVisitors(Guid organizationID);
    }
}

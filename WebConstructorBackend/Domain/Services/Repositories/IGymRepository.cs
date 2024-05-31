using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IGymRepository
    {
        public Gym CreateGym(Gym gym);

        public Gym UpdateGym(Guid id, Gym gym);

        public Gym DeleteGym(Guid id);

        public Gym GetGymById(Guid id);

        public List<Gym> GetOrganizationsGyms(Guid organizationID);
    }
}

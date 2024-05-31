using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class UsersTrainingsRepository : IUsersTrainingsRepository
    {
        public UsersTrainings CreateUsersTrainings(Guid userId, Training training)
        {
            throw new NotImplementedException();
        }

        public UsersTrainings GetTrainingInfo(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Training> GetUsersTrainingHistory(Guid userId)
        {
            throw new NotImplementedException();
        }

        public bool IsTrainingPayed(Guid userId, Guid trainingId)
        {
            throw new NotImplementedException();
        }

        public UsersTrainings UpdateUsersTraining(Guid id, UsersTrainings training)
        {
            throw new NotImplementedException();
        }
    }
}

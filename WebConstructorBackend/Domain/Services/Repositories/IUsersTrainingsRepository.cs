using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IUsersTrainingsRepository
    {
        public UsersTrainings GetTrainingInfo(Guid id);

        public bool IsTrainingPayed (Guid trainingId);

        public UsersTrainings UpdateUsersTraining(UsersTrainings training);

        public UsersTrainings CreateUsersTrainings(UsersTrainings training);
    }
}

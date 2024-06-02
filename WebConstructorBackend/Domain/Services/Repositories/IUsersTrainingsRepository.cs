using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IUsersTrainingsRepository
    {
        public UsersTrainings GetTrainingInfo(Guid userID, Guid trainingID);

        public bool IsTrainingPayed (Guid userID, Guid trainingID);

        public UsersTrainings UpdateUsersTraining(UsersTrainings training);

        public UsersTrainings CreateUsersTrainings(UsersTrainings training);
    }
}

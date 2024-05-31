using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public interface IUsersTrainingsRepository
    {
        public UsersTrainings GetTrainingInfo(Guid id);

        public List<Training> GetUsersTrainingHistory(Guid userId);

        public bool IsTrainingPayed (Guid userId, Guid trainingId);

        public UsersTrainings UpdateUsersTraining(Guid id, UsersTrainings training);

        public UsersTrainings CreateUsersTrainings(Guid userId, Training training);
    }
}

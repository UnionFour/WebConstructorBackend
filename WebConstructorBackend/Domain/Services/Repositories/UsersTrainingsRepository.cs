using WebConstructorBackend.Domain.Entities;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class UsersTrainingsRepository : IUsersTrainingsRepository
    {
        private readonly List<UsersTrainings> usersTrainings;

        public UsersTrainings CreateUsersTrainings(UsersTrainings training)
        {
            if (training.ID != null || training.ID.ToString() != "")
                usersTrainings.Add(training);

            return training;
        }

        public UsersTrainings GetTrainingInfo(Guid id)
        {
            return usersTrainings.FirstOrDefault(x => x.ID == id);
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

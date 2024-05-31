using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class UsersTrainingsRepository : IUsersTrainingsRepository
    {
        private readonly AppDBContext _db;
        public UsersTrainingsRepository([Service] AppDBContext db)
        {
            _db = db;
        }

        public UsersTrainings CreateUsersTrainings(UsersTrainings training)
        {
            if (training.ID != null || training.ID.ToString() != "")
            {
                _db.UsersTrainings.Add(training);
                _db.SaveChanges();
            }

            return training;
        }

        public UsersTrainings GetTrainingInfo(Guid id)
        {
            return _db.UsersTrainings.FirstOrDefault(x => x.ID == id);
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

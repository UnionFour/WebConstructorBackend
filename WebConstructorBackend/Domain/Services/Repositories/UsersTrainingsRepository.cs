using Microsoft.AspNetCore.Mvc;
using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class UsersTrainingsRepository : IUsersTrainingsRepository
    {
        private readonly AppDBContext _db;
        public UsersTrainingsRepository([FromServices] AppDBContext db)
        {
            _db = db;
        }

        public UsersTrainings CreateUsersTrainings(UsersTrainings training)
        {
            if ((training.TrainingID != null && training.UserID  != null) || (training.TrainingID.ToString() != "" && training.UserID.ToString() != ""))
            {
                _db.UsersTrainings.Add(training);
                _db.SaveChanges();
            }

            return training;
        }

        public UsersTrainings GetTrainingInfo(Guid userID, Guid trainingID)
        {
            return _db.UsersTrainings.FirstOrDefault(x => x.TrainingID == trainingID && x.UserID == userID);
        }

        public bool IsTrainingPayed(Guid userID, Guid trainingID)
        {
            return _db.UsersTrainings.FirstOrDefault(x => x.TrainingID == trainingID && x.UserID == userID).IsPaied;
        }

        public UsersTrainings UpdateUsersTraining(UsersTrainings training)
        {
            if (training != null)
            {
                _db.UsersTrainings.Update(training);
                _db.SaveChanges();
            }
            return training;
        }
    }
}

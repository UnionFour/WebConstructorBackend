using Microsoft.AspNetCore.Mvc;
using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly AppDBContext _db;
        public TrainingRepository([FromServices] AppDBContext db)
        {
            _db = db;
        }

        public Training CreateTraining(Training training)
        {
            if (training == null)
                return null;

            if (training.ID == Guid.Empty || training.ID.ToString() == null)
                training.ID = Guid.NewGuid();

            _db.Trainings.Add(training);
            _db.SaveChanges();

            return training;
        }

        public void DeleteTraining(Guid id)
        {
            var training = _db.Trainings.FirstOrDefault(x => x.ID == id);
            if (training != null)
            {
                _db.Trainings.Remove(training);
                _db.SaveChanges();
            }
        }

        public Training GetTraining(Guid id)
        {
            return _db.Trainings.FirstOrDefault(t => t.ID == id);
        }

        public Training UpdateTraining(Training training)
        {
            if (training != null)
            {
                _db.Trainings.Update(training);
                _db.SaveChanges();
            }
            return training;
        }
    }
}

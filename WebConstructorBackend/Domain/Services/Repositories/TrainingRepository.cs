using WebConstructorBackend.Domain.Entities;
using WebConstructorBackend.Domain.Services.DBContext;

namespace WebConstructorBackend.Domain.Services.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly AppDBContext _db;
        public TrainingRepository([Service] AppDBContext db)
        {
            _db = db;
        }

        public Training CreateTraining(Training training)
        {
            throw new NotImplementedException();
        }

        public void DeleteTraining(Guid id)
        {
            throw new NotImplementedException();
        }

        public Training GetTraining(Guid id)
        {
            throw new NotImplementedException();
        }

        public Training UpdateTraining(Guid id, Training training)
        {
            throw new NotImplementedException();
        }
    }
}

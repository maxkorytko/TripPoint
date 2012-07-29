using System;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public class DatabaseCheckpointRepository : DatabaseRepository, ICheckpointRepository
    {
        public DatabaseCheckpointRepository(TripPointDataContext dataContext)
            : base(dataContext)
        {

        }

        public Checkpoint FindCheckpoint(int checkpointID)
        {
            throw new NotImplementedException();
        }
    }
}

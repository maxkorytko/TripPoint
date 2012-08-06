using System;
using System.Linq;

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
            var checkpoint = (from c in DataContext.Checkpoints
                              where c.ID == checkpointID
                              select c)
                             .SingleOrDefault();

            return checkpoint;
        }
    }
}
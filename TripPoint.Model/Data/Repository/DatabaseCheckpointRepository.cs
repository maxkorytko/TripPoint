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

        public void SaveCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint == null) return;
            
            DataContext.Checkpoints.InsertOnSubmit(checkpoint);
            DataContext.SubmitChanges();
        }

        public void UpdateCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint == null) return;

            DataContext.SubmitChanges();
        }

        public void DeleteCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint == null) return;

            // ensure checkpoint is attached to the data context
            var checkpointEntity = FindCheckpoint(checkpoint.ID);

            if (checkpointEntity == null) return;

            checkpointEntity.Trip = null;
            DataContext.Notes.DeleteAllOnSubmit(checkpointEntity.Notes);
            DataContext.Checkpoints.DeleteOnSubmit(checkpointEntity);
            
            DataContext.SubmitChanges();
        }
    }
}
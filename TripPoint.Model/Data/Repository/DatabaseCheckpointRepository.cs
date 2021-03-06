﻿using System.Linq;
using System.Collections.Generic;

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
            DataContext.Locations.DeleteOnSubmit(checkpointEntity.Location);
            DataContext.Notes.DeleteAllOnSubmit(checkpointEntity.Notes);
            DataContext.Pictures.DeleteAllOnSubmit(checkpointEntity.Pictures);
            DataContext.Checkpoints.DeleteOnSubmit(checkpointEntity);
            
            DataContext.SubmitChanges();
        }

        public void DeleteCheckpoints(IEnumerable<Checkpoint> checkpoints)
        {
            // TODO: optimize to delete without looping using DataContext.DeleteAllOnSubmit
            foreach (var checkpoint in checkpoints)
            {
                DeleteCheckpoint(checkpoint);
            }
        }
    }
}
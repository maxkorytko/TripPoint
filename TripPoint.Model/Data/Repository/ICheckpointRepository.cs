using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface ICheckpointRepository
    {
        Checkpoint FindCheckpoint(int checkpointID);

        void SaveCheckpoint(Checkpoint checkpoint);
    }
}

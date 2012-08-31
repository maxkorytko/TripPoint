using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface ICheckpointRepository
    {
        /// <summary>
        /// Searches for a checkpoint
        /// </summary>
        /// <param name="checkpointID">checkpoint ID to search for</param>
        /// <returns></returns>
        Checkpoint FindCheckpoint(int checkpointID);

        /// <summary>
        /// Saves a new checkpoint in the data store
        /// </summary>
        /// <param name="checkpoint"></param>
        void SaveCheckpoint(Checkpoint checkpoint);

        /// <summary>
        /// Updates an existing checkpoint
        /// </summary>
        /// <param name="checkpoint"></param>
        void UpdateCheckpoint(Checkpoint checkpoint);

        /// <summary>
        /// Deletes a checkpoint from the data store
        /// </summary>
        /// <param name="checkpoint"></param>
        void DeleteCheckpoint(Checkpoint checkpoint);
    }
}

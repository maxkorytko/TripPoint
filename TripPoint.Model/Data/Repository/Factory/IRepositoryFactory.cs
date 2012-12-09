namespace TripPoint.Model.Data.Repository.Factory
{
    public interface IRepositoryFactory
    {
        ITripRepository TripRepository { get; }

        ICheckpointRepository CheckpointRepository { get; }

        IPictureRepository PictureRepository { get; }
    }
}

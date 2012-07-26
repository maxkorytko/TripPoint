namespace TripPoint.Model.Data.Repository.Factory
{
    public interface IRepositoryFactory
    {
        ITripRepository TripRepository { get; }
    }
}

namespace TripPoint.Model.Data.Repository.Factory
{
    public class DatabaseRepositoryFactory : IRepositoryFactory
    {
        private TripPointDataContext DataContext
        {
            get
            {
                return new TripPointDataContext(TripPointDataContext.ConnectionString);
            }
        }

        public ITripRepository TripRepository
        {
            get
            {
                return new DatabaseTripRepository(DataContext);
            }
        }
    }
}

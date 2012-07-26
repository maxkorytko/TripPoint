namespace TripPoint.Model.Data.Repository.Factory
{
    public class DatabaseRepositoryFactory : IRepositoryFactory
    {
        private TripPointDataContext _dataContext;

        private TripPointDataContext DataContext
        {
            get
            {
                if (_dataContext != null)
                    _dataContext.Dispose();

                _dataContext = new TripPointDataContext(TripPointDataContext.ConnectionString);

                return _dataContext;
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

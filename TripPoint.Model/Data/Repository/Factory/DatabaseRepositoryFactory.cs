namespace TripPoint.Model.Data.Repository.Factory
{
    public class DatabaseRepositoryFactory : IRepositoryFactory
    {
        private TripPointDataContext DataContext
        {
            get { return new TripPointDataContext(TripPointDataContext.ConnectionString); }
        }

        public ITripRepository TripRepository
        {
            get { return new DatabaseTripRepository(DataContext); }
        }

        public ICheckpointRepository CheckpointRepository
        {
            get { return new DatabaseCheckpointRepository(DataContext); }
        }

        public INoteRepository NoteRepository
        {
            get { return new DatabaseNoteRepository(DataContext); }
        }

        public IPictureRepository PictureRepository
        {
            get { return new DatabasePictureRepository(DataContext); }
        }
    }
}

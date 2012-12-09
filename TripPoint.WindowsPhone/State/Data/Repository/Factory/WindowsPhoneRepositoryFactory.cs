using System;

using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;

namespace TripPoint.WindowsPhone.State.Data.Repository.Factory
{
    public class WindowsPhoneRepositoryFactory : IRepositoryFactory
    {
        private IRepositoryFactory _repositoryFactory;

        public WindowsPhoneRepositoryFactory(IRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentException("repositoryFactory");

            _repositoryFactory = repositoryFactory;
        }

        public ITripRepository TripRepository
        {
            get { return _repositoryFactory.TripRepository; }
        }

        public ICheckpointRepository CheckpointRepository
        {
            get { return _repositoryFactory.CheckpointRepository; }
        }

        public IPictureRepository PictureRepository
        {
            get { return _repositoryFactory.PictureRepository ?? new IsolatedStoragePictureRepository(); }
        }
    }
}

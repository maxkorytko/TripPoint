using System;

namespace TripPoint.Model.Data.Repository.Factory
{
    /// <summary>
    /// Gateway for obtaining an instance of IRepositoryFactory
    /// </summary>
    public class RepositoryFactory
    {
        private static IRepositoryFactory _repositoryFactory;

        /// <summary>
        /// Wires up an implementation of IRepositoryFactory
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public static void Initialize(IRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");

            _repositoryFactory = repositoryFactory;
        }

        public static IRepositoryFactory Create()
        {
            return _repositoryFactory;
        }
    }
}

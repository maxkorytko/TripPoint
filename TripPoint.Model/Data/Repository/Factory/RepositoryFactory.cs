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
        /// Initializes abstract factory with a concrete factory
        /// </summary>
        /// <param name="repositoryFactory">concrete factory</param>
        public static void Initialize(IRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");

            _repositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Provides access to the concrete repository factory
        /// </summary>
        /// <returns>repository factory implementation</returns>
        public static IRepositoryFactory Create()
        {
            return _repositoryFactory;
        }
    }
}

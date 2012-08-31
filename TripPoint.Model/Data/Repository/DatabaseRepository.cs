using System;

namespace TripPoint.Model.Data.Repository
{
    /// <summary>
    /// Encapsulates common functionality across repositories backed up by a database
    /// </summary>
    public abstract class DatabaseRepository
    {
        public DatabaseRepository(TripPointDataContext dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException("dataContext");

            DataContext = dataContext;
        }

        protected TripPointDataContext DataContext { get; set; }
    }
}

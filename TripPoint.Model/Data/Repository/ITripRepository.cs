using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface ITripRepository
    {
        /// <summary>
        /// Persists trip to the data store
        /// </summary>
        /// <param name="trip">Trip object to persist</param>
        void SaveTrip(Trip trip);
    }
}

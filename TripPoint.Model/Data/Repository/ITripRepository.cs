using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface ITripRepository
    {
        IEnumerable<Trip> Trips { get; }

        /// <summary>
        /// Creates a trip in the data store
        /// Updates the trip if it already exists
        /// </summary>
        /// <param name="trip">Trip object to persist</param>
        void SaveTrip(Trip trip);

        Trip FindTrip(string tripID);

        void DeleteTrip(Trip trip);

        void DeleteAll();

        int Count { get; }
    }
}

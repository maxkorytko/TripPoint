using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface ITripRepository
    {
        IEnumerable<Trip> Trips { get; }

        Trip CurrentTrip { get; }

        Trip FindTrip(int tripID);

        /// <summary>
        /// Creates a trip in the data store
        /// Updates the trip if it already exists
        /// </summary>
        /// <param name="trip">Trip object to persist</param>
        void SaveTrip(Trip trip);

        void UpdateTrip(Trip trip);

        void DeleteTrip(Trip trip);
    }
}

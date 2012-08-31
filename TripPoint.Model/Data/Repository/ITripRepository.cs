using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface ITripRepository
    {
        /// <summary>
        /// Returns a collection of all trips in the data store
        /// </summary>
        IEnumerable<Trip> Trips { get; }

        /// <summary>
        /// Provides access to the current trip
        /// Current trip is a trip with no end date
        /// </summary>
        Trip CurrentTrip { get; }

        /// <summary>
        /// Searches for a trip
        /// </summary>
        /// <param name="tripID">trip ID to search for</param>
        /// <returns></returns>
        Trip FindTrip(int tripID);

        /// <summary>
        /// Creates a trip in the data store
        /// </summary>
        /// <param name="trip">Trip object to save</param>
        void SaveTrip(Trip trip);

        /// <summary>
        /// Updates an existing trip
        /// </summary>
        /// <param name="trip"></param>
        void UpdateTrip(Trip trip);

        /// <summary>
        /// Deletes a trip from the data store
        /// </summary>
        /// <param name="trip"></param>
        void DeleteTrip(Trip trip);
    }
}

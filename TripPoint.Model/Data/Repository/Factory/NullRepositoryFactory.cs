using System;
using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository.Factory
{
    public class NullRepositoryFactory : IRepositoryFactory
    {
        ITripRepository _tripRepository = new NullTripRepository();

        public ITripRepository TripRepository
        {
            get { return _tripRepository; }
        }
    }

    class NullTripRepository : ITripRepository
    {
        public IEnumerable<Trip> Trips
        {
            get { return new List<Trip>(); }
        }

        public Trip CurrentTrip
        {
            get { return new Trip(); }
        }

        public void SaveTrip(Trip trip)
        {
            // nothing
        }

        public Trip FindTrip(int tripID)
        {
            return new Trip();
        }

        public void DeleteTrip(Trip trip)
        {
            // nothing
        }
    }
}

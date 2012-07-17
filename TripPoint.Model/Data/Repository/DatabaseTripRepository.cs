using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;

using TripPoint.Model.Domain;
using TripPoint.Model.Utils;

namespace TripPoint.Model.Data.Repository
{
    public class DatabaseTripRepository : ITripRepository
    {
        public IEnumerable<Trip> Trips
        {
            get 
            {
                var trips = from trip in TripPointDataContext.DataContext.Trips
                            select trip;

                return trips;
            }
        }

        public void SaveTrip(Trip trip)
        {
            if (trip == null) return;

            var isNewTrip = FindTrip(trip.ID) == null;

            if (isNewTrip)
                TripPointDataContext.DataContext.Trips.InsertOnSubmit(trip);

            TripPointDataContext.DataContext.SubmitChanges();
        }

        public Trip FindTrip(int tripID)
        {
            var trip = (from t in TripPointDataContext.DataContext.Trips
                        where t.ID == tripID
                        select t)
                       .SingleOrDefault();
            
            return trip;
        }

        public void DeleteTrip(Trip trip)
        {
            if (trip == null) return;

            var tripToDelete = FindTrip(trip.ID);

            if (tripToDelete == null) return;

            TripPointDataContext.DataContext.Trips.DeleteOnSubmit(tripToDelete);
            TripPointDataContext.DataContext.SubmitChanges();
        }

        public int Count
        {
            get { return TripPointDataContext.DataContext.Trips.Count(); }
        }
    }
}

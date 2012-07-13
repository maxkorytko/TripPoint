using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;

using TripPoint.Model.Domain;
using TripPoint.Model.Utils;

namespace TripPoint.Model.Data.Repository
{
    public class IsolatedStorageTripRepository : ITripRepository
    {
        private TripPointDataContext _dataContext = TripPointDataContext.DataContext();

        public IEnumerable<Trip> Trips
        {
            get 
            {
                var trips = from trip in _dataContext.Trips
                            select trip;

                return trips;
            }
        }

        public void SaveTrip(Trip trip)
        {
            if (trip == null) return;

            var isNewTrip = FindTrip(trip.ID) == null;

            if (isNewTrip)
                _dataContext.Trips.InsertOnSubmit(trip);

            _dataContext.SubmitChanges();
        }

        public Trip FindTrip(int tripID)
        {
            var trip = (from t in _dataContext.Trips
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

            _dataContext.Trips.DeleteOnSubmit(tripToDelete);
            _dataContext.SubmitChanges();
        }

        public int Count
        {
            get { return _dataContext.Trips.Count(); }
        }
    }
}

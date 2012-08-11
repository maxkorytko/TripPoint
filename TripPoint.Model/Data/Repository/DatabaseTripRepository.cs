using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public class DatabaseTripRepository : DatabaseRepository, ITripRepository
    {
        public DatabaseTripRepository(TripPointDataContext dataContext)
            : base(dataContext)
        {
            // configure the data context to load associated checkpoints in a specific order

            var loadOptions = new DataLoadOptions();

            loadOptions.AssociateWith<Trip>(trip => trip.Checkpoints.OrderByDescending(
                checkpoint => checkpoint.Timestamp));

            DataContext.LoadOptions = loadOptions;
        }

        public IEnumerable<Trip> Trips
        {
            get 
            {
                var trips = from trip in DataContext.Trips
                            select trip;

                return trips;
            }
        }

        public Trip CurrentTrip
        {
            get
            {
                // there must be one and only one current trip
                return (from trip in Trips
                        where !trip.EndDate.HasValue
                        select trip).SingleOrDefault();
            }
        }

        public void SaveTrip(Trip trip)
        {
            if (trip == null) return;

            var isNewTrip = FindTrip(trip.ID) == null;

            if (isNewTrip)
            {
                DataContext.Trips.InsertOnSubmit(trip);
            }

            DataContext.SubmitChanges();
        }

        public Trip FindTrip(int tripID)
        {
            var trip = (from t in DataContext.Trips
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

            DataContext.Trips.DeleteOnSubmit(tripToDelete);
            DataContext.SubmitChanges();
        }
    }
}

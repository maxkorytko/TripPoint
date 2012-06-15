#region SDK Usings

using System;
using System.Collections.Generic;

#endregion

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository.Memory
{
    public class MemoryTripRepository : ITripRepository
    {
        private static IDictionary<string, Trip> _dataStore = CreateDataStore();

        #region Dummy Data
        private static IDictionary<string, Trip> CreateDataStore()
        {
            var dataStore = new Dictionary<string, Trip>();

            var trip = new Trip
            {
                Name = "Cabbot Trail",
                Notes = "Driving along Cabbot Trail in Nova Scotia",
                StartDate = DateTime.Now.AddDays(-4)
            };
            trip.Checkpoints.Add(new Checkpoint
            {
                Title = "Pearson in Toronto",
                Notes = "Minutes before departure"
            });
            trip.Checkpoints.Add(new Checkpoint
            {
                Title = "Going to Cabbot Trail"
            });
            trip.Checkpoints.Add(new Checkpoint
            {
                Title = "enjoying the scenery",
                Notes = "50 km along the trail"
            });
            dataStore[trip.ID] = trip;

            trip = new Trip
            {
                Name = "Hiking in Milton area",
                StartDate = DateTime.Now.AddDays(-25),
                EndDate = DateTime.Now.AddDays(-23)
            };
            trip.Checkpoints.Add(new Checkpoint
            {
                Title = "at the beginning of the trail"
            });
            trip.Checkpoints.Add(new Checkpoint
            {
                Title = "resting"
            });
            dataStore[trip.ID] = trip;

            return dataStore;
        }
        #endregion

        public void SaveTrip(Trip trip)
        {
            if (trip == null)
                throw new ArgumentNullException("trip");

            _dataStore[trip.ID] = trip;
        }

        public Trip FindTrip(string tripID)
        {
            if (tripID == null) return null;

            Trip trip = null;

            if (_dataStore.ContainsKey(tripID))
                trip = _dataStore[tripID];

            return trip;
        }

        public void DeleteTrip(Trip trip)
        {
            _dataStore.Remove(trip.ID);
        }

        public void DeleteAll()
        {
            _dataStore.Clear();
        }

        public int Count
        {
            get
            {
                return _dataStore.Count;
            }
        }
    }
}

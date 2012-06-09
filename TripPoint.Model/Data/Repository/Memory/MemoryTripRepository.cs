#region SDK Usings

using System;
using System.Collections.Generic;

#endregion

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository.Memory
{
    public class MemoryTripRepository : ITripRepository
    {
        private static IDictionary<string, Trip> _dataStore = new Dictionary<string, Trip>();

        public void SaveTrip(Trip trip)
        {
            if (trip == null)
                throw new ArgumentNullException("trip");

            _dataStore[trip.ID] = trip;

            //if (TripExists(trip))
            //{
            //    UpdateTrip(trip);
            //    return;
            //}

            //CreateTrip(trip);
        }

        //private static bool TripExists(Trip trip)
        //{
        //    return _dataStore.ContainsKey(trip.ID);
        //}

        //private static void CreateTrip(Trip trip)
        //{
        //    _dataStore.Add(trip.ID, trip);
        //}

        //private static void UpdateTrip(Trip trip)
        //{
        //    _dataStore[trip.ID] = trip;
        //}

        public Trip FindTrip(string tripID)
        {
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

#region SDK Usings

using System;

#endregion

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository.Memory
{
    public class MemoryTripRepository : ITripRepository
    {
        public void SaveTrip(Trip trip)
        {
            if (trip == null)
                throw new ArgumentNullException("trip");
        }
    }
}

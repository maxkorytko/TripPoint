using System.Data.Linq;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data
{
    public class TripPointDataContext : DataContext
    {
        public TripPointDataContext(string connectionString)
            : base(connectionString)
        { }

        public Table<Trip> Trips;

        public Table<Checkpoint> Checkpoints;
    }
}

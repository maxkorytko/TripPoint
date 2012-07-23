using System.Data.Linq;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data
{
    public class TripPointDataContext : DataContext
    {
        public static readonly string ConnectionString = "Data Source=isostore:/TripPoint.sdf;Max Database Size=256;";

        public TripPointDataContext(string connectionString)
            : base(connectionString)
        { }

        public Table<Trip> Trips;

        public Table<Checkpoint> Checkpoints;

        public Table<Note> Notes;
    }
}

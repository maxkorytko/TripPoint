using System;
using System.Windows.Navigation;

namespace TripPoint.WindowsPhone.Navigation
{
    /// <summary>
    /// Application URI mappings
    /// </summary>
    public class TripPointUriMappings
    {
        private static UriMapping[] _uriMappings = new UriMapping[]
        {
            new UriMapping
            {
                Uri = new Uri("/Trips", UriKind.Relative),
                MappedUri = new Uri("/View/Trip/TripListView.xaml", UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Trip/Create", UriKind.Relative),
                MappedUri = new Uri("/View/Trip/CreateTripView.xaml", UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Trip/Current", UriKind.Relative),
                MappedUri = new Uri("/View/Trip/CurrentTripView.xaml", UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Trip/{tripID}/Details", UriKind.Relative),
                MappedUri = new Uri("/View/Trip/TripDetailsView.xaml?tripID={tripID}", UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Trip/{tripID}/Checkpoints/Create", UriKind.Relative),
                MappedUri = new Uri("/View/Checkpoint/CreateCheckpointView.xaml?tripID={tripID}", UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Checkpoints/{checkpointID}/Details", UriKind.Relative),
                MappedUri = new Uri("/View/Checkpoint/CheckpointDetailsView.xaml?checkpointID={checkpointID}",
                    UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Trip/{tripID}/Checkpoints/{checkpointIndex}/Add/Notes", UriKind.Relative),
                MappedUri = new Uri("/View/Checkpoint/CheckpointAddNotesView.xaml?tripID={tripID}&checkpointIndex={checkpointIndex}",
                    UriKind.Relative)
            }
        };

        public static UriMapping[] UriMappings
        {
            get { return _uriMappings; }
        }
    }
}

using System;
using System.Windows.Navigation;

namespace TripPoint.WindowsPhone.Navigation
{
    /// <summary>
    /// Application URI mappings
    /// </summary>
    public class TripPointUriMappings
    {
        // all URI mappings
        // the order is important as the framework will pick the first matched URI
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
                Uri = new Uri("/Checkpoints/{checkpointID}/Edit", UriKind.Relative),
                MappedUri = new Uri("/View/Checkpoint/CheckpointEditView.xaml?checkpointID={checkpointID}",
                    UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Checkpoints/{checkpointID}/Add/Notes", UriKind.Relative),
                MappedUri = new Uri("/View/Checkpoint/CheckpointAddNotesView.xaml?checkpointID={checkpointID}",
                    UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Checkpoints/{checkpointID}/Add/Pictures", UriKind.Relative),
                MappedUri = new Uri("/View/Checkpoint/CheckpointAddPicturesView.xaml?checkpointID={checkpointID}",
                    UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Pictures/{pictureID}/Details", UriKind.Relative),
                MappedUri = new Uri("/View/Picture/PictureDetailsView.xaml?pictureID={pictureID}",
                    UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Application/Settings", UriKind.Relative),
                MappedUri = new Uri("/View/Settings/ApplicationSettingsView.xaml", UriKind.Relative)
            },
            new UriMapping
            {
                Uri = new Uri("/Privacy", UriKind.Relative),
                MappedUri = new Uri("/View/Settings/PrivacyPolicyView.xaml", UriKind.Relative)
            }
        };

        public static UriMapping[] UriMappings
        {
            get { return _uriMappings; }
        }
    }
}

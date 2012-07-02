#region SDK Usings

using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

#endregion

using TripPoint.Model.Utils;
using TripPoint.WindowsPhone;
using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.Navigation
{
    /// <summary>
    /// Navigation facade for the application
    /// </summary>
    public class TripPointNavigation
    {
        private static UriMapper _uriMapper;

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
                Uri = new Uri("/Trip/{tripID}/Checkpoints/{checkpointIndex}/Add/Notes", UriKind.Relative),
                MappedUri = new Uri("/View/Checkpoint/CheckpointAddNotesView.xaml?tripID={tripID}&checkpointIndex={checkpointIndex}",
                    UriKind.Relative)
            }
        };

        /// <summary>
        /// UriMapper mapping all URIs for the application
        /// </summary>
        public static UriMapper UriMapper
        {
            get
            {
                InitializeUriMapper();
                return _uriMapper;
            }
        }

        /// <summary>
        /// Reference to the last navigated page
        /// Allows accessing the properties of the current page from a view model
        /// </summary>
        public static PhoneApplicationPage CurrentPage { get; private set; }

        private static void InitializeUriMapper()
        {
            if (_uriMapper != null) return;

            _uriMapper = new UriMapper();

            foreach (UriMapping uriMapping in _uriMappings)
            {
                _uriMapper.UriMappings.Add(uriMapping);
            }
        }

        /// <summary>
        /// Navigates to a previous page if possible
        /// </summary>
        public static void GoBack()
        {
            if ((Application.Current as App).RootFrame.CanGoBack)
                (Application.Current as App).RootFrame.GoBack();
        }

        /// <summary>
        /// Navigates to a given URI
        /// </summary>
        /// <param name="uri">URI to navigate to</param>
        /// <returns>true if navigation is successful</returns>
        public static bool Navigate(string uri)
        {
            var rootFrame = (Application.Current as App).RootFrame;

            return rootFrame.Navigate(new Uri(uri, UriKind.Relative));
        }

        /// <summary>
        /// Listener for the Navigated event
        /// Sets the reference to the last navigated content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Navigated(Object sender, NavigationEventArgs e)
        {
            PhoneApplicationPage page = null;

            if (!(e.Content is PhoneApplicationPage)) return;
            
            page = e.Content as PhoneApplicationPage;

            // TODO: remove the static property
            CurrentPage = page;

            if (page.DataContext != null && (page.DataContext is TripPointViewModelBase))
            {
                var navigationEventArgs = new TripPointNavigationEventArgs(e, page);

                (page.DataContext as TripPointViewModelBase).OnNavigatedTo(navigationEventArgs);
            }
        }
    }
}

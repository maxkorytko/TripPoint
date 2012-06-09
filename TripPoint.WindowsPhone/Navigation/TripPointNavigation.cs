#region SDK Usings

using System;
using System.Windows;
using System.Windows.Navigation;

#endregion

using TripPoint.WindowsPhone;

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
            new UriMapping()
            {
                Uri = new Uri("/TripDetails/{ID}", UriKind.Relative),
                MappedUri = new Uri("/View/Trip/TripDetailsView.xaml", UriKind.Relative)
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
        /// Navigates to a given URI
        /// </summary>
        /// <param name="uri">URI to navigate to</param>
        /// <returns>true if navigation is successful</returns>
        public static bool Navigate(string uri)
        {
            var rootFrame = (Application.Current as App).RootFrame;

            return rootFrame.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}

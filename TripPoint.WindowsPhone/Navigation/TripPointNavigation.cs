using System;
using System.Windows.Navigation;

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
                Uri = new Uri("/TripDetails", UriKind.Relative),
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
    }
}

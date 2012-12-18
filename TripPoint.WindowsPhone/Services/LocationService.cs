using System;
using System.Device.Location;

namespace TripPoint.WindowsPhone.Services
{
    public class LocationService
    {
        public const string ENABLED_SETTING_KEY             = "LocationService.IsEnabled";
        public const bool   ENABLED_SETTING_DEFAULT_VALUE   = true;

        private GeoCoordinateWatcher _geoCoordinateWatcher;

        public LocationService()
        {
            _geoCoordinateWatcher = new GeoCoordinateWatcher();
        }

        /// <summary>
        /// Has the service been started?
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Permission to access location service
        /// </summary>
        public GeoPositionPermission Permission
        {
            get { return _geoCoordinateWatcher.Permission; }
        }

        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;

        public void Start()
        {
            if (IsRunning) return;

            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High); 
            _geoCoordinateWatcher.MovementThreshold = 20;
            _geoCoordinateWatcher.PositionChanged += PositionChanged;
            
            _geoCoordinateWatcher.Start();

            IsRunning = true;
        }

        public void Stop()
        {
            _geoCoordinateWatcher.Stop();

            IsRunning = false;
        }
    }
}

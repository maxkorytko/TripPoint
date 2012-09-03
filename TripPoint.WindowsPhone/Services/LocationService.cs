using System;
using System.Device.Location;

namespace TripPoint.WindowsPhone.Services
{
    public class LocationService
    {
        private GeoCoordinateWatcher _geoCoordinateWatcher;

        public LocationService()
        {
            _geoCoordinateWatcher = new GeoCoordinateWatcher();
        }

        public bool IsRunning { get; private set; }
        
        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;

        public void Start()
        {
            if (IsRunning) return;

            _geoCoordinateWatcher.MovementThreshold = 15.0;
            _geoCoordinateWatcher.PositionChanged += PositionChanged;

            _geoCoordinateWatcher.Start();

            IsRunning = true;
        }

        public void Stop()
        {
            _geoCoordinateWatcher.Stop();
            _geoCoordinateWatcher.PositionChanged -= PositionChanged;

            IsRunning = false;
        }
    }
}

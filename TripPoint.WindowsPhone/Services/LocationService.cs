using System;
using System.Windows;
using System.Device.Location;

namespace TripPoint.WindowsPhone.Services
{
    public class LocationService
    {
        private GeoCoordinateWatcher _geoCoordinateWatcher;

        public bool IsRunning { get { return _geoCoordinateWatcher != null; } }

        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;

        public void Start()
        {
            if (IsRunning) return;

            _geoCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default); 
            _geoCoordinateWatcher.MovementThreshold = 20;
            _geoCoordinateWatcher.PositionChanged += (sender, args) =>
            {
                TripPoint.Model.Utils.Logger.Log(
                    "* Location: {0}, {1}, Timestamp: {2}",
                    args.Position.Location.Latitude,
                    args.Position.Location.Longitude,
                    args.Position.Timestamp);

                if (PositionChanged != null)
                    PositionChanged(sender, args);
            };
            
            _geoCoordinateWatcher.Start();
        }

        public void Stop()
        {
            if (_geoCoordinateWatcher == null) return;

            _geoCoordinateWatcher.Stop();
            _geoCoordinateWatcher = null;
        }
    }
}

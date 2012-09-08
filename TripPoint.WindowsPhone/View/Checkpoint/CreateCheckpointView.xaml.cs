using System;
using System.Windows;
using System.Windows.Navigation;
using System.Device.Location;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.View.Checkpoint
{
    public partial class CreateCheckpointView : PhoneApplicationPage
    {
        private EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> _positionChangedEventHandler = null;

        public CreateCheckpointView()
        {
            InitializeComponent();

            _positionChangedEventHandler = (sender, args) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (ViewModel != null)
                    {
                        ViewModel.SaveCheckpointLocation(args.Position.Location);
                    }
                });
            };
        }

        private CreateCheckpointViewModel ViewModel
        {
            get { return (DataContext as CreateCheckpointViewModel); }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ViewModel == null) return;

            ViewModel.LocationService.PositionChanged += _positionChangedEventHandler;

            // start obtaining the location as soon as the view is ready,
            // in order to speed up checkpoint creation
            // there is a good chance that the location will have been obtained 
            // by the time the user saves a checkpoint
            //
            if (ViewModel.LocationService.Permission == GeoPositionPermission.Granted)
                ViewModel.LocationService.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (ViewModel == null) return;

            ViewModel.LocationService.Stop();
            ViewModel.LocationService.PositionChanged -= _positionChangedEventHandler;
        }
    }
}
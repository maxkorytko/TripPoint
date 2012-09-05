using System;
using System.Windows;
using System.Windows.Navigation;
using System.Device.Location;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;
using TripPoint.WindowsPhone.Services;
using TripPoint.WindowsPhone.I18N;

namespace TripPoint.WindowsPhone.View.Checkpoint
{
    public partial class CreateCheckpointView : PhoneApplicationPage
    {
        private LocationService _locationService;

        public CreateCheckpointView()
        {
            InitializeComponent();
            InitializeLocationService();
        }

        private void InitializeLocationService()
        {
            if (_locationService != null) return;

            _locationService = new LocationService();

            EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> positionChangedEventHandler = null;
            positionChangedEventHandler = (sender, args) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (DataContext != null)
                    {
                        var viewModel = DataContext as CreateCheckpointViewModel;
                        viewModel.SaveCheckpointLocation(args.Position.Location);
                    }
                });
            };

            _locationService.PositionChanged += positionChangedEventHandler;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // start obtaining the location as soon as the view is ready,
            // in order to speed up checkpoint creation
            // there is a good chance that the location will have been obtained 
            // by the time the user saves a checkpoint
            //
            if (_locationService.Permission == GeoPositionPermission.Granted)
            {
                _locationService.Start();
                return;
            }

            DisplayLocationServiceDeniedMessage();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            _locationService.Stop();
        }

        private void DisplayLocationServiceDeniedMessage()
        {
            var resources = new Localization().Resources;

            MessageBox.Show(I18N.Resources.LocationServiceTurnedOffText,
                 I18N.Resources.LocationServiceTurnedOffCaption, MessageBoxButton.OK);
        }
    }
}
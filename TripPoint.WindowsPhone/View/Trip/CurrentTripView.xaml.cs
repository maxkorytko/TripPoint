using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.View.Controls;

namespace TripPoint.WindowsPhone.View.Trip
{
    public partial class CurrentTripView : PhoneApplicationPage
    {
        public CurrentTripView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.Back)
            {
                (DataContext as CurrentTripViewModel).OnBackNavigatedTo();
            }
        }

        private void CheckpointList_OnCheckpointSelected(object sender, CheckpointSelectedEventArgs args)
        {
            (DataContext as CurrentTripViewModel).ViewCheckpointDetailsCommand.Execute(args.SelectedCheckpoint);
        }

        private void CheckpointList_OnMore(object sender, RoutedEventArgs args)
        {
            (DataContext as CurrentTripViewModel).PaginateCheckpointsCommand.Execute(null);
        }
    }
}
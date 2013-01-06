using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;
using TripPoint.WindowsPhone.View.Controls;

namespace TripPoint.WindowsPhone.View.Trip
{
    public partial class TripDetailsView : PhoneApplicationPage
    {
        public TripDetailsView()
        {
            InitializeComponent();
        }

        private TripDetailsViewModel ViewModel
        {
            get { return DataContext as TripDetailsViewModel; }
        }

        private void CheckpointList_OnCheckpointSelected(object sender, CheckpointSelectedEventArgs args)
        {
            ViewModel.ViewCheckpointDetailsCommand.Execute(args.SelectedCheckpoint);
        }

        private void CheckpointList_OnMore(object sender, RoutedEventArgs args)
        {
            ViewModel.PaginateCheckpointsCommand.Execute(null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.OnNavigatedTo(new Navigation.TripPointNavigationEventArgs(e));
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            ViewModel.Checkpoints = null;
        }
    }
}
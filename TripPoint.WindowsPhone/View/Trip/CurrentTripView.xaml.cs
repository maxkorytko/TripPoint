using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;
using TripPoint.WindowsPhone.Navigation;

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

            (DataContext as CurrentTripViewModel).OnNavigatedTo(new TripPointNavigationEventArgs(e));
        }

        /// <summary>
        /// View model does not know anything about the view, so resetting the selected item here
        /// Resetting the selected item allows the user to select the same item more than once
        /// This is necessary for the case when there is only one item in the list
        /// </summary>
        private void CheckpointList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox.SelectedItem == null) return;

            (DataContext as CurrentTripViewModel).ViewCheckpointDetailsCommand.Execute(listBox.SelectedItem);

            // allow selecting the same item multiple times
            listBox.SelectedItem = null;
        }
    }
}
using System.Windows.Controls;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.View.Trip
{
    public partial class TripListView : PhoneApplicationPage
    {
        public TripListView()
        {
            InitializeComponent();
        }

        private void TripList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox.SelectedItem == null) return;

            (DataContext as TripListViewModel).ViewTripDetailsCommand.Execute(listBox.SelectedItem);

            listBox.SelectedItem = null;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            (DataContext as TripListViewModel).OnNavigatedTo(new Navigation.TripPointNavigationEventArgs(e));
        }
    }
}
using System.Windows.Controls;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.View.Trip
{
    public partial class TripDetailsView : PhoneApplicationPage
    {
        public TripDetailsView()
        {
            InitializeComponent();
        }

        private void CheckpointList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox.SelectedItem == null) return;

            (DataContext as TripDetailsViewModel).ViewCheckpointDetailsCommand.Execute(listBox.SelectedItem);

            // allow selecting the same item multiple times
            listBox.SelectedItem = null;
        }
    }
}
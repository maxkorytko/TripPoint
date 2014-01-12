using Microsoft.Phone.Controls;
using System.Windows.Navigation;

using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.View.Checkpoint
{
    public partial class CheckpointAddPicturesView : PhoneApplicationPage
    {
        public CheckpointAddPicturesView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs args)
        {
            base.OnNavigatingFrom(args);

            if (args.NavigationMode == NavigationMode.Back)
            {
                (DataContext as CheckpointAddPicturesViewModel).ResetViewModel();
            }
        }
    }
}
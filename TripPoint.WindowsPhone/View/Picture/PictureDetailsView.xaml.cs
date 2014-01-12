using Microsoft.Phone.Controls;
using System.Windows.Navigation;

using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.View.Picture
{
    public partial class PictureDetailsView : PhoneApplicationPage
    {
        public PictureDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs args)
        {
            base.OnNavigatingFrom(args);

            if (args.NavigationMode == NavigationMode.Back)
            {
                (DataContext as PictureDetailsViewModel).ResetViewModel();
            }
        }
    }
}
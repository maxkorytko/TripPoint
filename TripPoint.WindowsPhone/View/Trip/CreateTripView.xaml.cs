using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using TripPoint.WindowsPhone.ViewModel;
using TripPoint.WindowsPhone.I18N;

namespace TripPoint.WindowsPhone.View.Trip
{
    public partial class CreateTripView : PhoneApplicationPage
    {
        public CreateTripView()
        {
            InitializeComponent();
        }

        private CreateTripViewModel ViewModel
        {
            get { return (DataContext as CreateTripViewModel); }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CreateApplicationBar();
        }

        private void CreateApplicationBar()
        {
            if (ApplicationBar != null) return;

            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.IsVisible = true;

            CreateSaveButton();
            CreateCancelButton();
        }

        private void CreateSaveButton()
        {
            ApplicationBarIconButton saveButton = new ApplicationBarIconButton();
            saveButton.IconUri = new Uri("/Assets/Images/Dark/appbar.save.rest.png", UriKind.Relative);
            saveButton.Text = I18N.Resources.Save;
            saveButton.Click += (sender, args) => OnSaveButtonClicked();

            ApplicationBar.Buttons.Add(saveButton);
        }

        private void CreateCancelButton()
        {
            if (ViewModel == null || !ViewModel.CanCancelCreateTrip) return;

            ApplicationBarIconButton cancelButton = new ApplicationBarIconButton();
            cancelButton.IconUri = new Uri("/Assets/Images/Dark/appbar.stop.rest.png", UriKind.Relative);
            cancelButton.Text = I18N.Resources.Cancel;
            cancelButton.Click += (sender, args) => OnCancelButtonClicked();

            ApplicationBar.Buttons.Add(cancelButton);
        }

        private void OnSaveButtonClicked()
        {
            if (ViewModel == null) return;

            ViewModel.SaveTripCommand.Execute(null);
        }

        private void OnCancelButtonClicked()
        {
            if (ViewModel == null) return;

            ViewModel.CancelCreateTripCommand.Execute(null);
        }
    }
}
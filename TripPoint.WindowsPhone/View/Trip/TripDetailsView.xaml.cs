using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;
using TripPoint.WindowsPhone.View.Controls;

namespace TripPoint.WindowsPhone.View.Trip
{
    public partial class TripDetailsView : PhoneApplicationPage
    {
        private ImageBrush _backgroundImageBrush;

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

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            ViewModel.Checkpoints = null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.OnNavigatedTo(new Navigation.TripPointNavigationEventArgs(e));

            if (e.NavigationMode == NavigationMode.New) LoadBackgroundImage();
        }

        private void LoadBackgroundImage()
        {
            if (_backgroundImageBrush != null)
            {
                LoadBackgroundImage(_backgroundImageBrush);
                return;
            }

            var imageSource = new BitmapImage(new Uri("/Assets/Images/background.jpg", UriKind.Relative));
            imageSource.CreateOptions = BitmapCreateOptions.BackgroundCreation;

            imageSource.ImageOpened += (sender, args) =>
            {
                _backgroundImageBrush = new ImageBrush();
                _backgroundImageBrush.ImageSource = imageSource;

                LoadBackgroundImage(_backgroundImageBrush);
            };
        }

        private void LoadBackgroundImage(ImageBrush background)
        {
            if (background == null) return;

            TripDetailsPanorama.Background = background;
            FadeInBackgroundImage();
        }

        private void FadeInBackgroundImage()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromSeconds(1.0));;
            animation.From = 0.00;
            animation.To = 1.00;

            var storyboard = new Storyboard();
            storyboard.Duration = animation.Duration;
            storyboard.Children.Add(animation);
            
            Storyboard.SetTarget(animation, TripDetailsPanorama);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            storyboard.Begin();
        }
    }
}
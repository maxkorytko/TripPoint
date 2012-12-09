using Microsoft.Phone.Controls;
using System.Device.Location;
using System.Windows.Navigation;

using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace TripPoint.WindowsPhone.View.Checkpoint
{
    public partial class CheckpointDetailsView : PhoneApplicationPage
    {
        public CheckpointDetailsView()
        {
            InitializeComponent();
            RegisterMessages();
            Unloaded += (sender, args) => OnUnloaded();
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<PropertyChangedMessage<bool>>(this, message =>
            {
                if (message.PropertyName.Equals("ShouldShowCheckpointMap"))
                {
                    UpdateCheckpointMapVisibility(message.NewValue);
                }
            });
        }

        private void UpdateCheckpointMapVisibility(bool isVisible)
        {
            Logger.Log(this, "Map is visible: {0}", isVisible);

            if (isVisible)
                ShowCheckpointMap();
            else
                HideCheckpointMap();
        }

        private void ShowCheckpointMap()
        {
            var mapCenter = (DataContext as CheckpointDetailsViewModel).Checkpoint.Location.GeoCoordinate ??
                            GeoCoordinate.Unknown;

            // we don't want to display the map unless we known the checkpoint location
            if (mapCenter == GeoCoordinate.Unknown) return;

            CheckpointMap.MapCenter = mapCenter;
            ShowPivotItem(CheckpointMapPivotItem);
        }

        private void ShowPivotItem(PivotItem item)
        {
            if (item == null) return;

            if (CheckpointDetails.Items.IndexOf(item) == -1)
                CheckpointDetails.Items.Add(item);
        }

        private void HideCheckpointMap()
        {
            CheckpointMap.MapCenter = GeoCoordinate.Unknown;
            HidePivotItem(CheckpointMapPivotItem);
        }

        private void HidePivotItem(PivotItem item)
        {
            if (item == null) return;

            CheckpointDetails.Items.Remove(item);
        }

        private void OnUnloaded()
        {
            Messenger.Default.Unregister(this);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // we must reset the view model to free memory allocated for thumbnails
            (DataContext as CheckpointDetailsViewModel).ResetViewModel();
        }
    }
}
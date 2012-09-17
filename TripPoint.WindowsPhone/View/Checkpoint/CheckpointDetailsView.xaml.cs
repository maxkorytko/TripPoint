using Microsoft.Phone.Controls;

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
            TripPoint.Model.Utils.Logger.Log(this, "Map is visible: {0}", isVisible);

            if (isVisible)
                ShowPivotItem(CheckpointMap);
            else
                HidePivotItem(CheckpointMap);
        }

        private void ShowPivotItem(PivotItem item)
        {
            if (item == null) return;

            if (CheckpointDetails.Items.IndexOf(item) == -1)
                CheckpointDetails.Items.Add(item);
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
    }
}
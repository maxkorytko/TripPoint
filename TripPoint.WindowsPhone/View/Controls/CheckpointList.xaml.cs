using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace TripPoint.WindowsPhone.View.Controls
{
    public partial class CheckpointList : UserControl
    {
        public CheckpointList()
        {
            InitializeComponent();
        }

        public delegate void CheckpointSelectedEventHandler(object sender, CheckpointSelectedEventArgs args);
        public event CheckpointSelectedEventHandler OnCheckpointSelected;

        public delegate void MoreEventHandler(object sender, RoutedEventArgs args);
        public event MoreEventHandler OnMore;

        /// <summary>
        /// Resetting the selected item allows the user to select the same item more than once
        /// This is especially important when there is only one item in the list
        /// </summary>
        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as LongListSelector;

            if (listBox.SelectedItem == null) return;

            if (OnCheckpointSelected != null)
            {
                var eventArgs = new CheckpointSelectedEventArgs(listBox.SelectedItem as Model.Domain.Checkpoint);
                OnCheckpointSelected(this, eventArgs);
            }

            // allow selecting the same item multiple times
            listBox.SelectedItem = null;
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnMore != null) OnMore(this, e);
        }
    }

    public class CheckpointSelectedEventArgs : EventArgs
    {
        public CheckpointSelectedEventArgs(Model.Domain.Checkpoint selectedCheckpoint)
        {
            SelectedCheckpoint = selectedCheckpoint;
        }

        public object SelectedCheckpoint { get; private set; }
    }
}

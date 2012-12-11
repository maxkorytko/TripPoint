using System;
using System.Windows.Controls;

using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.State.Data;

namespace TripPoint.WindowsPhone.View.Controls
{
    public partial class PictureGrid : UserControl
    {
        public PictureGrid()
        {
            InitializeComponent();
        }

        public delegate void PictureSelectedEventHandler(object sender, PictureSelectedEventArgs args);

        public event PictureSelectedEventHandler OnPictureSelected;

        private void PictureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pictureListBox = sender as ListBox;

            if (pictureListBox.SelectedItem == null) return;

            if (OnPictureSelected != null)
            {
                var selectedPicture = pictureListBox.SelectedItem;

                OnPictureSelected(this, new PictureSelectedEventArgs(selectedPicture));
            }

            pictureListBox.SelectedItem = null;
        }
    }

    public class PictureSelectedEventArgs : EventArgs
    {
        public PictureSelectedEventArgs(object selectedPicture)
        {
            SelectedPicture = selectedPicture;
        }

        public object SelectedPicture { get; private set; }
    }
}

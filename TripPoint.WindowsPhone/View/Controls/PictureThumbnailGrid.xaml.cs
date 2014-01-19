using System.Windows.Controls;

using TripPoint.Model.Domain;
using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.View.Controls
{
    public partial class PictureThumbnailGrid : UserControl
    {
        public PictureThumbnailGrid()
        {
            InitializeComponent();
        }
        
        private void PictureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pictureListBox = sender as ListBox;

            if (pictureListBox.SelectedItem == null) return;

            ViewPictureFromThumbnail(pictureListBox.SelectedItem as Thumbnail);

            pictureListBox.SelectedItem = null;
        }

        private void ViewPictureFromThumbnail(Thumbnail thumbnail)
        {
            if (thumbnail == null) return;

            (DataContext as CheckpointDetailsViewModel).ViewPictureCommand.Execute(thumbnail.Picture);
        }
    }
}

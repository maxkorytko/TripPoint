using System;
using System.Windows.Media;

using TripPoint.Model.Domain;
using TripPoint.WindowsPhone.Utils;

namespace TripPoint.WindowsPhone.State.Data
{
    public class PictureThumbnail : Thumbnail
    {
        public PictureThumbnail(Uri imageUri)
            : this(imageUri, null)
        {   
        }

        public PictureThumbnail(Uri imageUri, Picture picture)
            : base(imageUri)
        {
            _picture = picture;
        }

        private Picture _picture;

        private byte[] _pictureBytes;

        public Picture Picture
        {
            get { return _picture; }
            set
            {
                if (_picture == value) return;

                _picture = value;
                _pictureBytes = null;
            }
        }

        public override ImageSource Source
        {
            get 
            {
                LoadThumbnailForPicture();

                return base.Source;
            }
        }

        private void LoadThumbnailForPicture()
        {
            if (Picture == null) return;

            if (_pictureBytes != null) return;

            _pictureBytes = Picture.RawBytes ?? PictureStateManager.Instance.LoadPicture(Picture);

            Source = ImageUtils.CreateBitmapFromBytes(_pictureBytes);
        }
    }
}

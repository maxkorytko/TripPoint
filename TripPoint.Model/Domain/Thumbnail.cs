using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TripPoint.Model.Domain
{
    public class Thumbnail
    {
        private Uri _imageUri;

        private ImageSource _source;

        public Thumbnail(Uri imageUri)
        {
            if (imageUri == null) throw new ArgumentException("imageUri");

            _imageUri = imageUri;
        }

        public virtual ImageSource Source
        {
            get 
            {
                if (_source == null)
                    _source = new BitmapImage(_imageUri);

                return _source; 
            }
            protected set
            {
                if (_source == value) return;

                _source = value;
                NotifyPropertyChanged("Source");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}

using System;
using System.ComponentModel;
using System.Windows.Media;

using TripPoint.WindowsPhone.Utils;

namespace TripPoint.Model.Domain
{
    public class Thumbnail : INotifyPropertyChanged
    {
        private ImageSource _source;

        public Thumbnail(Picture picture)
        {
            if (picture == null)
                throw new ArgumentException("picture");

            Picture = picture;
        }

        public Picture Picture { get; private set; }

        public ImageSource Source
        {
            get 
            {
                if (_source == null)
                    PictureLoader.Instance.LoadThumbnail(this);

                return _source; 
            }
            set
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

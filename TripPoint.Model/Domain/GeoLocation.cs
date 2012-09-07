using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// Represens geographical location
    /// </summary>
    ///
    [Table]
    public class GeoLocation : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _locationID;
        private double _latitude;
        private double _longitude;

        public static GeoLocation Unknown
        {
            get { return new GeoLocation(0.0, 0.0); }
        }

        public GeoLocation(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        #region ID
        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert
        )]
        public int ID
        {
            get
            {
                return _locationID;
            }
            set
            {
                if (_locationID != value)
                {
                    NotifyPropertyChanging("ID");
                    _locationID = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }
        #endregion

        #region Latitude
        [Column]
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude == value) return;

                NotifyPropertyChanging("Latitude");
                _latitude = value;
                NotifyPropertyChanged("Latitude");
            }
        }
        #endregion

        #region Longitude
        [Column]
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude == value) return;

                NotifyPropertyChanging("Longitude");
                _longitude = value;
                NotifyPropertyChanged("Longitude");
            }
        }
        #endregion

        #region Version
        [Column(IsVersion = true)]
        private Binary _version;
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion 
    }
}

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Device.Location;

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
            get { return new GeoLocation { Latitude = 0.0, Longitude = 0.0 }; }
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

        public bool IsUnknown
        {
            get { return Latitude == 0.0 && Longitude == 0.0; }
        }

        public GeoCoordinate GeoCoordinate
        {
            get { return new GeoCoordinate(Latitude, Longitude); }
        }

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

using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq.Mapping;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// Represents a checkpoint along the trip route
    /// </summary>
    [Table]
    [Index(Columns="Timestamp DESC")]
    public class Checkpoint : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _checkpointID;
        private DateTime _timestamp;
        private string _title;
        
        private EntitySet<Note> _notes;
        private EntityRef<Trip> _trip;
        private EntityRef<GeoLocation> _location;

        public Checkpoint()
        {
            Timestamp = DateTime.Now;
            Location = GeoLocation.Unknown;
            _notes = new EntitySet<Note>(AddNoteAction, RemoveNoteAction);
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
                return _checkpointID;
            }
            set
            {
                if (_checkpointID != value)
                {
                    NotifyPropertyChanging("ID");
                    _checkpointID = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }
        #endregion

        #region Timestamp
        /// <summary>
        /// Checkpoint creation time and date
        /// </summary>
        [Column]
        public DateTime Timestamp 
        {
            get { return _timestamp; }
            set
            {
                if (_timestamp == value) return;

                NotifyPropertyChanging("Timestamp");
                _timestamp = value;
                NotifyPropertyChanged("Timestamp");
            }
        }
        #endregion

        #region Title
        [Column]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;

                NotifyPropertyChanging("Title");
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }
        #endregion

        #region Notes
        [Association(Storage = "_notes", OtherKey = "_checkpointID", ThisKey = "ID")]
        public EntitySet<Note> Notes
        {
            get { return _notes; }
            set
            {
                _notes.Assign(value);
            }
        }

        // Called during an add operation
        private void AddNoteAction(Note note)
        {
            NotifyPropertyChanging("Note");
            note.Checkpoint = this;
        }

        // Called during a remove operation
        private void RemoveNoteAction(Note note)
        {
            NotifyPropertyChanging("Note");
            note.Checkpoint = null;
        }
        #endregion

        #region Trip
        [Column]
        internal int _tripID;

        [Association(
            Storage = "_trip",
            ThisKey = "_tripID",
            OtherKey = "ID",
            IsForeignKey = true
        )]
        public Trip Trip
        {
            get { return _trip.Entity; }
            set
            {
                NotifyPropertyChanging("Trip");
                _trip.Entity = value;

                if (value != null)
                    _tripID = value.ID;

                NotifyPropertyChanged("Trip");
            }
        }
        #endregion

        #region Location
        [Column]
        internal int _locationID;

        [Association(
            Storage = "_location",
            ThisKey = "_locationID",
            OtherKey = "ID",
            IsForeignKey = true
        )]
        public GeoLocation Location
        {
            get { return _location.Entity; }
            set
            {
                NotifyPropertyChanging("Location");
                _location.Entity = value;

                if (value != null)
                    _locationID = value.ID;

                NotifyPropertyChanged("Location");
            }
        }
        #endregion

        #region Version
        [Column(IsVersion = true)]
        private Binary _version;
        #endregion

        public override string ToString()
        {
            return string.Format("{0} at {1}. Location: {2}",
                Title, Timestamp.TimeOfDay, Location);
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

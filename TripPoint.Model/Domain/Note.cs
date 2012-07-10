using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// A note with the ability to track the time and date it was taken on
    /// </summary>
    [Table]
    public class Note : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _noteID;
        private string _text;
        private DateTime _timestamp;

        private EntityRef<Checkpoint> _checkpoint;

        public Note()
        {
            Timestamp = DateTime.Now;
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
                return _noteID;
            }
            set
            {
                if (_noteID != value)
                {
                    NotifyPropertyChanging("ID");
                    _noteID = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }
        #endregion

        #region Text
        [Column]
        public string Text
        {
            get { return _text;}
            set
            {
                if (_text == value) return;

                NotifyPropertyChanging("Text");
                _text = value;
                NotifyPropertyChanged("Text");
            }
        }
        #endregion

        /// <summary>
        /// The time and date the note was creted on
        /// </summary>
        #region Timestamp
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

        #region Checkpoint
        [Column]
        internal int _checkpointID;

        [Association(
            Storage = "_checkpoint",
            ThisKey = "_checkpointID",
            OtherKey = "ID",
            IsForeignKey = true
        )]
        public Checkpoint Checkpoint
        {
            get { return _checkpoint.Entity; }
            set
            {
                NotifyPropertyChanging("Checkpoint");
                _checkpoint.Entity = value;

                if (value != null)
                    _checkpointID = value.ID;

                NotifyPropertyChanged("Checkpoint");
            }
        }
        #endregion

        #region Version
        [Column(IsVersion = true)]
        private Binary _version;
        #endregion

        public override string ToString()
        {
            return string.Format("'{0}' on {1}", Text, Timestamp);
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

using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Phone.Data.Linq.Mapping;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// Represents a trip
    /// </summary>
    [Table]
    [Index(Columns="EndDate DESC")]
    public class Trip : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _tripID;
        private string _name;
        private string _place;
        private string _notes;
        private DateTime _startDate;
        private DateTime? _endDate;
        private EntitySet<Checkpoint> _checkpoints;

        public Trip()
        {
            Name = string.Empty;
            StartDate = DateTime.Now;
            EndDate = null;
            Notes = string.Empty;
            
            _checkpoints = new EntitySet<Checkpoint>(AddCheckpointAction,
                RemoveCheckpointAction);
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
                return _tripID;
            }
            set
            {
                if (_tripID != value)
                {
                    NotifyPropertyChanging("ID");
                    _tripID = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }
        #endregion

        #region Name
        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;

                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }
        #endregion

        #region Place
        [Column]
        public string Place
        {
            get { return _place; }
            set
            {
                if (_place == value) return;

                NotifyPropertyChanging("Place");
                _place = value;
                NotifyPropertyChanged("Place");
            }
        }
        #endregion

        #region StartDate
        [Column]
        public DateTime StartDate
        {
            get { return _startDate;}
            set
            {
                if (_startDate == value) return;

                NotifyPropertyChanging("StartDate");
                _startDate = value;
                NotifyPropertyChanged("StartDate");
            }
        }
        #endregion

        #region EndDate
        [Column]
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;

                NotifyPropertyChanging("EndDate");
                _endDate = value;
                NotifyPropertyChanged("EndDate");
            }
        }
        #endregion

        #region Notes
        [Column]
        public string Notes
        {
            get { return _notes; }
            set
            {
                if (_notes == value) return;

                NotifyPropertyChanging("Notes");
                _notes = value;
                NotifyPropertyChanged("Notes");
            }
        }
        #endregion

        #region Checkpoints
        [Association(Storage = "_checkpoints", OtherKey = "_tripID", ThisKey = "ID")]
        public EntitySet<Checkpoint> Checkpoints
        {
            get { return _checkpoints; }
            set 
            { 
                _checkpoints.Assign(value); 
            }
        }

        // Called during an add operation
        private void AddCheckpointAction(Checkpoint checkpoint)
        {
            NotifyPropertyChanging("Checkpoint");
            checkpoint.Trip = this;
        }

        // Called during a remove operation
        private void RemoveCheckpointAction(Checkpoint checkpoint)
        {
            NotifyPropertyChanging("Checkpoint");
            checkpoint.Trip = null;
        }
        #endregion

        #region Version
        [Column(IsVersion = true)]
        private Binary _version;
        #endregion

        /// <summary>
        /// Validates required properties
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return IsNameValid() && IsStartDateValid() && IsEndDateValid();
        }

        #region Validation
        
        private bool IsNameValid()
        {
            return !string.IsNullOrEmpty(Name);
        }

        private bool IsStartDateValid()
        {
            return StartDate <= DateTime.Now;
        }

        private bool IsEndDateValid()
        {
            // TODO: implement
            return true;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Name: {0}, StartDate: {1}", Name, StartDate);
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

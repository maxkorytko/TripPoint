using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Windows.Media;
using Microsoft.Phone.Data.Linq.Mapping;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Media.Imaging;

namespace TripPoint.Model.Domain
{
    [Table]
    public class Picture : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _pictureID;
        private string _fileName;
        private string _title;
        private DateTime _dateTaken;
        private byte[] _rawBytes;
        
        private EntityRef<Checkpoint> _checkpoint;

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
                return _pictureID;
            }
            set
            {
                if (_pictureID != value)
                {
                    NotifyPropertyChanging("ID");
                    _pictureID = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }
        #endregion

        #region FileName
        [Column]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value) return;

                NotifyPropertyChanging("FileName");
                _fileName = value;
                NotifyPropertyChanged("FileName");
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

        #region DateTaken
        [Column]
        public DateTime DateTaken
        {
            get { return _dateTaken; }
            set
            {
                if (_dateTaken == value) return;

                NotifyPropertyChanging("DateTaken");
                _dateTaken = value;
                NotifyPropertyChanged("DateTaken");
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

        #region RawBytes
        public byte[] RawBytes
        {
            get { return _rawBytes; }
            set
            {
                if (_rawBytes == value) return;

                _rawBytes = value;
                NotifyPropertyChanged("RawBytes");
            }
        }
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

﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Device.Location;

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.Services;
using TripPoint.WindowsPhone.Utils;
using TripPoint.I18N;
using TripPoint.Model.Data.Repository;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateCheckpointViewModel : Base.TripViewModelBase
    {
        private Checkpoint _checkpoint;
        private string _notes;
        private bool _isWaitingForLocation;
        private LocationService _locationService;
        private CountdownTimer _timer;
        
        public CreateCheckpointViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            _locationService = new LocationService();
            _timer = new CountdownTimer();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
            CancelCreateCheckpointCommand = new RelayCommand(CancelCreateCheckpointAction);
        }

        public LocationService LocationService { get { return _locationService; } }

        public Checkpoint Checkpoint 
        {
            get { return _checkpoint; }
            private set
            {
                if (_checkpoint == value) return;

                _checkpoint = value;
                RaisePropertyChanged("Checkpoint");
            }
        }

        public string Notes
        {
            get { return _notes; }
            set
            {
                if (_notes == value) return;

                _notes = value;
                RaisePropertyChanged("Notes");
            }
        }

        public bool IsWaitingForLocation
        {
            get { return _isWaitingForLocation; }
            set
            {
                if (_isWaitingForLocation == value) return;

                _isWaitingForLocation = value;
                RaisePropertyChanged("IsWaitingForLocation");
            }
        }

        public ICommand CreateCheckpointCommand { get; private set; }

        public ICommand CancelCreateCheckpointCommand { get; private set; }

        private void CreateCheckpointAction()
        {
            if (Checkpoint == null) return;
                
            AddNotesToCheckpoint();
            Checkpoint.Timestamp = DateTime.Now;

            if (LocationService.Permission == GeoPositionPermission.Denied)
            {
                // inform the user that the location service is turned off on the device
                // this is not be confused with turning off the location service in app settings
                // in the latter case, the app should save the checkpoint without asking for user's permission
                //
                DisplayLocationNotFoundMessage();
                return;
            }
            
            if (!IsLocationObtained() && LocationService.IsRunning)
            {
                WaitUntilLocationIsObtained(15);
                return;
            }

            SaveCheckpoint();
            GoBack();
        }

        /// <summary>
        /// Assigns notes, if any, to the checkpoint being created
        /// </summary>
        private void AddNotesToCheckpoint()
        {
            if (String.IsNullOrEmpty(Notes)) return;

            Checkpoint.Notes.Add(
               new Note { Text = Notes }
            );
        }

        private bool IsLocationObtained()
        {
            return Checkpoint.Location != null && !Checkpoint.Location.IsUnknown;
        }

        private void SaveCheckpoint()
        {
            if (Trip == null || TripRepository == null) return;

            Trip.Checkpoints.Add(Checkpoint);
            TripRepository.UpdateTrip(Trip);
        }

        private void GoBack()
        {
            ResetViewModel();
            Navigator.GoBack();
        }

        private void DisplayLocationNotFoundMessage()
        {
            var userDecision = MessageBox.Show(Resources.LocationNotFoundText,
                Resources.LocationNotFoundCaption,
                MessageBoxButton.OKCancel);

            if (userDecision != MessageBoxResult.OK) return;

            SaveCheckpoint();
            GoBack();
        }

        private void WaitUntilLocationIsObtained(int timeout)
        {
            IsWaitingForLocation = true;

            _timer.Stop();

            _timer.Interval = TimeSpan.FromSeconds(timeout);
            _timer.OnExplode = () =>
            {
                if (!IsViewTopMost) return;

                IsWaitingForLocation = false;
                DisplayLocationNotFoundMessage();
            };

            _timer.Start();
        }

        /// <summary>
        /// A handler method which gets called when the location is available from the location service
        /// </summary>
        /// <param name="location"></param>
        public void OnLocationObtained(GeoCoordinate location)
        {
            SaveCheckpointLocation(location);

            if (IsWaitingForLocation)
            {
                StopWaitingForLocation();
                SaveCheckpoint();
                GoBack();
            }
        }

        /// <summary>
        /// Assigns the location to the checkpoint being created
        /// </summary>
        /// <param name="location"></param>
        public void SaveCheckpointLocation(GeoCoordinate location)
        {
            if (location == null || location.IsUnknown) return;
            if (Checkpoint == null) return;

            Checkpoint.Location = new GeoLocation
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        public override void ResetViewModel()
        {
            base.ResetViewModel();

            Checkpoint = null;
            Notes = String.Empty;
        }

        private void CancelCreateCheckpointAction()
        {
            GoBack();
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            InitializeCheckpoint();
            StopWaitingForLocation();
        }

        private void InitializeCheckpoint()
        {
            if (Checkpoint != null) return;

            Checkpoint = new Checkpoint();
        }

        private void StopWaitingForLocation()
        {
            _timer.Stop();
            IsWaitingForLocation = false;
        }
    }
}

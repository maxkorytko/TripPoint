#region SDK Usings

using System;
using System.Windows;
using System.Windows.Input;
using System.Device.Location;
using Microsoft.Phone.Controls;

#endregion

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.Services;
using TripPoint.WindowsPhone.Utils;
using TripPoint.WindowsPhone.I18N;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateCheckpointViewModel : TripPointViewModelBase
    {
        private int _tripID = -1;
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
            AddPicturesCommand = new RelayCommand(AddPicturesAction);
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

        public ICommand AddPicturesCommand { get; private set; }

        private void CreateCheckpointAction()
        {
            if (Checkpoint == null)
            {
                CloseView();
                return;
            }
                
            AddNotesToCheckpoint();

            if (IsLocationObtained())
            {
                SaveCheckpoint();
                CloseView();
            }
            else if (LocationService.Permission == GeoPositionPermission.Denied)
                DisplayLocationNotFoundMessage();
            else
                WaitUntilLocationIsObtained(10);
        }

        /// <summary>
        /// Assigns notes, if any, to the checkpoint being created
        /// </summary>
        private void AddNotesToCheckpoint()
        {
            if (string.IsNullOrEmpty(Notes)) return;

            Checkpoint.Notes.Add(
               new Note { Text = Notes }
            );
        }

        private bool IsLocationObtained()
        {
            return Checkpoint.Location != null;
        }

        private void SaveCheckpoint()
        {
            var tripRepository = RepositoryFactory.TripRepository;
            var trip = tripRepository.FindTrip(_tripID);

            if (trip == null) return;

            trip.Checkpoints.Add(Checkpoint);
            tripRepository.UpdateTrip(trip);
        }

        private void CloseView()
        {
            ResetViewModel();
            Navigator.GoBack();
        }

        private void DisplayLocationNotFoundMessage()
        {
            var userDecision = MessageBox.Show(Resources.LocationNotFoundText,
                Resources.LocationNotFoundCaption,
                MessageBoxButton.OKCancel);

            if (userDecision == MessageBoxResult.OK)
                SaveCheckpoint();

            CloseView();
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
                CloseView();
            }
        }

        /// <summary>
        /// Assigns the location to the checkpoint being created
        /// </summary>
        /// <param name="location"></param>
        public void SaveCheckpointLocation(GeoCoordinate location)
        {
            if (location == null || location.IsUnknown) return;

            Logger.Log("Location: {0}, {1}", location.Latitude, location.Longitude);
            
            Checkpoint.Location = new GeoLocation
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        private void StopWaitingForLocation()
        {
            _timer.Stop();
            IsWaitingForLocation = false;
        }

        /// <summary>
        /// Sets default values to properties intended for UI binding
        /// This ensures that the user will not see previous values if he/she opens the view again
        /// </summary>
        private void ResetViewModel()
        {
            Checkpoint = null;
            Notes = string.Empty;
        }

        private void CancelCreateCheckpointAction()
        {
            CloseView();
        }

        private void AddPicturesAction()
        {
            Logger.Log(this, "Add pictures");
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            StopWaitingForLocation();
            
            InitializeTripID(e.View);
            InitializeCheckpoint();
        }

        private void InitializeTripID(PhoneApplicationPage view)
        {
            if (view == null) return;

            var tripID = view.TryGetQueryStringParameter("tripID");

            if (string.IsNullOrWhiteSpace(tripID)) return;

            _tripID = TripPointConvert.ToInt32(tripID);
        }

        private void InitializeCheckpoint()
        {
            if (Checkpoint != null) return;

            Checkpoint = new Checkpoint
            {
                Timestamp = DateTime.Now
            };
        }
    }
}

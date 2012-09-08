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
using TripPoint.WindowsPhone.I18N;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateCheckpointViewModel : TripPointViewModelBase
    {
        private int _tripID = -1;
        private Checkpoint _checkpoint;
        private string _notes;
        private LocationService _locationService;
        
        public CreateCheckpointViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            _locationService = new LocationService();

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

            if (IsLocationReady())
            {
                SaveCheckpoint();
                CloseView();
            }
            else if (LocationService.Permission == GeoPositionPermission.Denied)
                DisplayLocationServiceDeniedMessage();
            else
                WaitUntilLocationIsReady(20);
        }

        private void AddNotesToCheckpoint()
        {
            if (string.IsNullOrEmpty(Notes)) return;

            Checkpoint.Notes.Add(
               new Note { Text = Notes }
            );
        }

        private bool IsLocationReady()
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

        private static void DisplayLocationServiceDeniedMessage()
        {
            var resources = new Localization().Resources;

            MessageBox.Show(I18N.Resources.LocationNotFoundText, I18N.Resources.LocationNotFoundCaption,
                MessageBoxButton.OK);
        }

        private void WaitUntilLocationIsReady(int timeout)
        {

        }

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

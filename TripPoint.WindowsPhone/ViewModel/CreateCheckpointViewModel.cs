#region SDK Usings

using System;
using System.Windows;
using System.Windows.Input;
using System.Device.Location;
using Microsoft.Phone.Controls;

#endregion

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.Services;

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
            InitializeCommands();
            InitializeLocationService();
        }

        private void InitializeCommands()
        {
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
            CancelCreateCheckpointCommand = new RelayCommand(CancelCreateCheckpointAction);
            AddPicturesCommand = new RelayCommand(AddPicturesAction);
        }

        private void InitializeLocationService()
        {
            if (_locationService != null) return;

            _locationService = new LocationService();

            EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> positionChangedEventHandler = null;
            positionChangedEventHandler = (sender, args) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => 
                {
                    StopLocationService();
                    SaveCheckpointLocation(args.Position.Location);
                });
            };
            
            _locationService.PositionChanged += positionChangedEventHandler;
        }

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
            if (Checkpoint != null)
            {
                AddNotesToCheckpoint();
                SaveCheckpoint();
            }

            ResetViewModel();
            Navigator.GoBack();
        }

        private void AddNotesToCheckpoint()
        {
            if (string.IsNullOrEmpty(Notes)) return;

            Checkpoint.Notes.Add(
               new Note { Text = Notes }
            );
        }

        private void SaveCheckpoint()
        {
            var tripRepository = RepositoryFactory.TripRepository;
            var trip = tripRepository.FindTrip(_tripID);

            if (trip == null) return;

            trip.Checkpoints.Add(Checkpoint);
            tripRepository.UpdateTrip(trip);
        }

        private void SaveCheckpointLocation(GeoCoordinate location)
        {
            if (location == null) return;

            Logger.Log("Location: {0}, {1}", location.Latitude, location.Longitude);
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
            ResetViewModel();
            Navigator.GoBack();
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

            // start obtaining the location as soon as the view is ready,
            // in order to speed up checkpoint creation
            // there is a good chance that the location will have been obtained 
            // by the time the user saves a checkpoint
            //
            StartLocationService();
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

        private void StartLocationService()
        {
            if (_locationService == null) return;

            _locationService.Start();
        }

        private void StopLocationService()
        {
            if (_locationService == null) return;

            _locationService.Stop();
        }
    }
}

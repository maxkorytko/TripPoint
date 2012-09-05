#region SDK Usings

using System;
using System.Windows.Input;
using System.Device.Location;
using Microsoft.Phone.Controls;

#endregion

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateCheckpointViewModel : TripPointViewModelBase
    {
        private int _tripID = -1;
        private Checkpoint _checkpoint;
        private string _notes;
        
        public CreateCheckpointViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
            CancelCreateCheckpointCommand = new RelayCommand(CancelCreateCheckpointAction);
            AddPicturesCommand = new RelayCommand(AddPicturesAction);
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

        public void SaveCheckpointLocation(GeoCoordinate location)
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

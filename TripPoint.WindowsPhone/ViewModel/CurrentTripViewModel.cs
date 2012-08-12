#region SDK Usings
using System.Windows.Input;
using System.Linq;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CurrentTripViewModel : TripPointViewModelBase
    {
        private Trip _currentTrip;
        private Checkpoint _latestCheckpoint;
        private bool _currentTripHasCheckpoints;
        private bool _currentTripHasNoCheckpoints;
        
        public CurrentTripViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            FinishTripCommand = new RelayCommand(FinishTripAction);
            PastTripsCommand = new RelayCommand(PastTripsAction);
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
            AddNotesCommand = new RelayCommand(AddNotesAction);
            AddPicturesCommand = new RelayCommand(AddPicturesAction);
            ViewCheckpointDetailsCommand = new RelayCommand<Checkpoint>(ViewCheckpointDetailsAction);
        }

        public Trip CurrentTrip
        {
            get { return _currentTrip; }
            private set
            {
                if (_currentTrip == value) return;

                _currentTrip = value;
                RaisePropertyChanged("CurrentTrip");
            }
        }

        public Checkpoint LatestCheckpoint
        {
            get { return _latestCheckpoint; }
            private set
            {
                if (_latestCheckpoint == value) return;

                _latestCheckpoint = value;
                RaisePropertyChanged("LatestCheckpoint");
            }
        }

        public bool CurrentTripHasCheckpoints
        {
            get { return _currentTripHasCheckpoints; }
            private set
            {
                if (_currentTripHasCheckpoints == value) return;

                _currentTripHasCheckpoints = value;
                RaisePropertyChanged("CurrentTripHasCheckpoints");
            }
        }

        public bool CurrentTripHasNoCheckpoints
        {
            get { return _currentTripHasNoCheckpoints; }
            private set
            {
                if (_currentTripHasNoCheckpoints == value) return;

                _currentTripHasNoCheckpoints = value;
                RaisePropertyChanged("CurrentTripHasNoCheckpoints");
            }
        }

        public ICommand FinishTripCommand { get; private set; }

        public ICommand PastTripsCommand { get; private set; }

        public ICommand CreateCheckpointCommand { get; private set; }

        public ICommand AddNotesCommand { get; private set; }

        public ICommand AddPicturesCommand { get; private set; }

        public ICommand ViewCheckpointDetailsCommand { get; private set; }

        private void FinishTripAction()
        {
            Logger.Log(this, "Finish Trip");
        }

        private void PastTripsAction()
        {
            Navigator.Navigate("/Trips");
        }

        private void CreateCheckpointAction()
        {
            Navigator.Navigate(
                string.Format("/Trip/{0}/Checkpoints/Create", CurrentTrip.ID));
        }

        private void AddNotesAction()
        {
            Navigator.Navigate(
                string.Format("/Checkpoints/{0}/Add/Notes", LatestCheckpoint.ID));
        }

        private void AddPicturesAction()
        {
            Logger.Log(this, "Add Pictures");
        }

        private void ViewCheckpointDetailsAction(Checkpoint checkpoint)
        {
            Navigator.Navigate(string.Format("/Checkpoints/{0}/Details", checkpoint.ID));
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SetCurrentTrip();
            SetLatestCheckpoint();
            SetCurrentTripHasCheckpoints();
        }

        private void SetCurrentTrip()
        {
            var tripRepository = RepositoryFactory.TripRepository;
            
            if (tripRepository == null) return;

            var trip = tripRepository.CurrentTrip;

            if (trip == null)
                trip = new Trip();

            CurrentTrip = trip;
        }

        private void SetLatestCheckpoint()
        {
            if (CurrentTrip == null) return;

            LatestCheckpoint = CurrentTrip.Checkpoints.FirstOrDefault();
        }

        private void SetCurrentTripHasCheckpoints()
        {
            if (CurrentTrip == null) return;

            CurrentTripHasCheckpoints = CurrentTrip.Checkpoints.Count > 0;
            CurrentTripHasNoCheckpoints = !CurrentTripHasCheckpoints;
        }
    }
}

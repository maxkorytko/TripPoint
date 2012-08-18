#region SDK Usings
using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.I18N;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CurrentTripViewModel : TripPointViewModelBase
    {
        private ITripRepository _tripRepository;
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
            var userDecision = MessageBox.Show(Resources.ConfirmFinishTrip, Resources.Confirm,
                MessageBoxButton.OKCancel);

            if (userDecision == MessageBoxResult.OK)
                FinishCurrentTrip();
        }

        private void FinishCurrentTrip()
        {
            CurrentTrip.EndDate = DateTime.Now;
            _tripRepository.SaveTrip(CurrentTrip);

            Navigator.Navigate("/Trips");
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

            _tripRepository = RepositoryFactory.TripRepository;

            InitializeCurrentTrip();
            InitializeLatestCheckpoint();
            InitializeCurrentTripHasCheckpoints();
        }

        private void InitializeCurrentTrip()
        {
            if (_tripRepository == null) return;

            CurrentTrip = _tripRepository.CurrentTrip;
        }

        private void InitializeLatestCheckpoint()
        {
            if (CurrentTrip == null) return;

            LatestCheckpoint = CurrentTrip.Checkpoints.FirstOrDefault();
        }

        private void InitializeCurrentTripHasCheckpoints()
        {
            if (CurrentTrip == null) return;

            CurrentTripHasCheckpoints = CurrentTrip.Checkpoints.Count > 0;
            CurrentTripHasNoCheckpoints = !CurrentTripHasCheckpoints;
        }
    }
}

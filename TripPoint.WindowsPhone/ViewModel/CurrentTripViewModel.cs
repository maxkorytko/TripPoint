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
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CurrentTripViewModel : TripPointViewModelBase
    {
        private IRepositoryFactory _repositoryFactory;
        private ITripRepository _tripRepository;

        public CurrentTripViewModel(IRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");

            _repositoryFactory = repositoryFactory;
            _tripRepository = repositoryFactory.TripRepository;

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
            get
            {
                var currentTrip = _tripRepository.CurrentTrip;

                return currentTrip != null ? currentTrip : new Trip();
            }
        }

        public bool CurrentTripHasCheckpoints
        {
            get
            {
                return CurrentTrip.Checkpoints.Count > 0;
            }
        }

        public bool CurrentTripHasNoCheckpoints
        {
            get
            {
                return !CurrentTripHasCheckpoints;
            }
        }

        public Checkpoint LatestCheckpoint
        {
            get
            {
                if (CurrentTripHasNoCheckpoints) return null;

                return CurrentTrip.Checkpoints.LastOrDefault();
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
            TripPointNavigation.Navigate("/Trips");
        }

        private void CreateCheckpointAction()
        {
            TripPointNavigation.Navigate(
                string.Format("/Trip/{0}/Checkpoints/Create", CurrentTrip.ID));
        }

        private void AddNotesAction()
        {
            TripPointNavigation.Navigate(
                string.Format("/Trip/{0}/Checkpoints/{1}/Add/Notes", CurrentTrip.ID, 0));
        }

        private void AddPicturesAction()
        {
            Logger.Log(this, "Add Pictures");
        }

        private void ViewCheckpointDetailsAction(Checkpoint checkpoint)
        {
            Logger.Log(this, "view checkpoint: {0}", checkpoint);

            var tripID = "0";
            var checkpointID = "0";

            // TODO: implement
            TripPointNavigation.Navigate(string.Format("/Trip/{0}/Checkpoints/{1}/Details",
                tripID, checkpointID));
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _tripRepository = _repositoryFactory.TripRepository;
        }
    }
}

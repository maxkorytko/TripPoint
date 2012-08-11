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
        private ITripRepository _tripRepository;

        public CurrentTripViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");

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

                return CurrentTrip.Checkpoints.First();
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
            var checkpoint = CurrentTrip.Checkpoints.FirstOrDefault<Checkpoint>();

            Navigator.Navigate(
                string.Format("/Checkpoints/{0}/Add/Notes", checkpoint.ID));
        }

        private void AddPicturesAction()
        {
            Logger.Log(this, "Add Pictures");
        }

        private void ViewCheckpointDetailsAction(Checkpoint checkpoint)
        {
            Logger.Log(this, "view checkpoint: {0}", checkpoint);

            Navigator.Navigate(string.Format("/Checkpoints/{0}/Details", checkpoint.ID));
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _tripRepository = RepositoryFactory.TripRepository;
        }
    }
}

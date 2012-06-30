#region SDK Usings
using System.Windows;
using System.Windows.Input;
using System.Linq;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CurrentTripViewModel : TripPointViewModelBase
    {
        public CurrentTripViewModel()
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
        }

        public Trip CurrentTrip
        {
            get
            {
                Trip currentTrip = (Application.Current as App).CurrentTrip;

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

        private void FinishTripAction()
        {
            Logger.Log(this, "Finish Trip");
        }

        private void PastTripsAction()
        {
            Logger.Log(this, "Past Trips");
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
    }
}

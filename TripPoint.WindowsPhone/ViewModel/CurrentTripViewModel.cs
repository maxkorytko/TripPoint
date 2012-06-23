#region SDK Usings
using System.Windows;
using System.Windows.Input;
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
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
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

        public ICommand FinishTripCommand { get; private set; }

        public ICommand CreateCheckpointCommand { get; private set; }

        private void FinishTripAction()
        {
            Logger.Log(this, "Finish Trip");
        }

        private void CreateCheckpointAction()
        {
            TripPointNavigation.Navigate(
                string.Format("/Trip/{0}/Checkpoints/Create", CurrentTrip.ID));
        }
    }
}

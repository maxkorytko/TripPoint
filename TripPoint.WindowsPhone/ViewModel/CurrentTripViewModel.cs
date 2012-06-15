#region SDK Usings
using System.Windows;
using System.Windows.Input;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CurrentTripViewModel : TripPointViewModelBase
    {
        public CurrentTripViewModel()
        {
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
        }

        public Trip CurrentTrip
        {
            get
            {
                Trip currentTrip = (Application.Current as App).CurrentTrip;

                if (currentTrip == null)
                    currentTrip = new Trip();

                return currentTrip;
            }
        }

        public ICommand CreateCheckpointCommand { get; private set; }

        private void CreateCheckpointAction()
        {
            Logger.Log(this, "Create Checkpoint");
        }
    }
}

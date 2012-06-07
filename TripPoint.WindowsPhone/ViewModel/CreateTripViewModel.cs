using System;
using TripPoint.Model.Domain;
using TripPoint.Model.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateTripViewModel : TripPointViewModelBase
    {
        public CreateTripViewModel()
        {
            Trip = new Trip
            {
                StartDate = DateTime.Now
            };
            
            SaveTripCommand = new RelayCommand(SaveTrip);
        }

        public Trip Trip { get; private set; }

        public RelayCommand SaveTripCommand { get; private set; }

        private void SaveTrip()
        {
            Logger.Log(this, "save trip: {0}, {1}, {2}", Trip.Name, Trip.StartDate, Trip.Notes);
        }
    }
}

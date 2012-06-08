#region SDK Usings

using System;
using System.Windows;
using Microsoft.Phone.Controls;

#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateTripViewModel : TripPointViewModelBase
    {
        public CreateTripViewModel()
        {
            Trip = new Trip();
            
            SaveTripCommand = new RelayCommand(SaveTrip);
        }

        public Trip Trip { get; private set; }

        public RelayCommand SaveTripCommand { get; private set; }

        private void SaveTrip()
        {
            Logger.Log(this, "save trip: {0}, {1}, {2}", Trip.Name, Trip.StartDate, Trip.Notes);

            bool isTripValid = Trip.Validate();

            if (isTripValid)
            {
                PersistTrip();

                var rootVisual = Application.Current.RootVisual as PhoneApplicationFrame;
                rootVisual.Navigate(new Uri("/TripDetails", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Validation Failed");
            }
        }

        private void PersistTrip()
        {
            //_tripRepository.SaveTrip(Trip);
        }
    }
}

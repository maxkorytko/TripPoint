#region SDK Usings

using System;
using System.Windows;
using Microsoft.Phone.Controls;

#endregion

using TripPoint.WindowsPhone.Navigation;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateTripViewModel : TripPointViewModelBase
    {
        /// <summary>
        /// The repository for persisting trips
        /// </summary>
        private ITripRepository _tripRepository;

        public CreateTripViewModel(ITripRepository tripRepository)
        {
            if (tripRepository == null)
                throw new ArgumentNullException("tripRepository");

            _tripRepository = tripRepository;

            InitProperties();
        }

        private void InitProperties()
        {
            Trip = new Trip();
            SaveTripCommand = new RelayCommand(SaveTrip);
        }

        /// <summary>
        /// The trip being created
        /// </summary>
        public Trip Trip { get; private set; }

        /// <summary>
        /// Command invoked on trip save action
        /// </summary>
        public RelayCommand SaveTripCommand { get; private set; }

        private void SaveTrip()
        {
            bool isTripValid = Trip.Validate();

            if (isTripValid)
            {
                PersistTrip();
                TripPointNavigation.Navigate("/Trip/Current");
            }
            else
            {
                MessageBox.Show("Validation Failed");
            }
        }

        private void PersistTrip()
        {
            _tripRepository.SaveTrip(Trip);

            Logger.Log("Persisted trip: {0}", Trip);
        }
    }
}

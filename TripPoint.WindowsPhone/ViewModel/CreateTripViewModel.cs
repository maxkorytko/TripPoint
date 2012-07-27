#region SDK Usings

using System;
using System.Windows;
using Microsoft.Phone.Controls;

#endregion

using TripPoint.WindowsPhone.Navigation;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
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

        public CreateTripViewModel(IRepositoryFactory repositoryFactory)
                : base(repositoryFactory)
        {
            _tripRepository = RepositoryFactory.TripRepository;

            Trip = new Trip();
            SaveTripCommand = new RelayCommand(SaveTripAction);
        }

        /// <summary>
        /// The trip being created
        /// </summary>
        public Trip Trip { get; private set; }

        /// <summary>
        /// Command invoked on trip save action
        /// </summary>
        public RelayCommand SaveTripCommand { get; private set; }

        private void SaveTripAction()
        {
            bool isTripValid = Trip.Validate();

            if (!isTripValid)
            {
                MessageBox.Show("Validation Failed");
                return;
            }

            SaveTrip();
            
            Navigator.Navigate("/Trip/Current");
        }

        private void SaveTrip()
        {
            _tripRepository.SaveTrip(Trip);

            Logger.Log("Persisted trip: {0}", Trip);
        }
    }
}

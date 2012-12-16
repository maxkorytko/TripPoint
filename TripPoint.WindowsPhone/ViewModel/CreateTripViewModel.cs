﻿#region SDK Usings
using System.Windows;
using System.Windows.Input;
using System.Linq;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateTripViewModel : Base.TripPointViewModelBase
    {
        private Trip _trip;
        private ITripRepository _tripRepository;

        public CreateTripViewModel(IRepositoryFactory repositoryFactory)
                : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveTripCommand = new RelayCommand(SaveTripAction);
            CancelCreateTripCommand = new RelayCommand(CancelCreateTripAction);
        }

        private void SaveTripAction()
        {
            bool isTripValid = Trip.Validate();

            if (!isTripValid)
            {
                MessageBox.Show("Validation Failed");
                return;
            }

            SaveTrip();

            Navigator.ClearHistory();
            Navigator.NavigateWithoutHistory("/Trip/Current");
        }

        private void SaveTrip()
        {
            if (_tripRepository == null) return;

            _tripRepository.SaveTrip(Trip);
        }

        private void CancelCreateTripAction()
        {
            Navigator.GoBack();
        }

        public Trip Trip
        {
            get { return _trip; }
            set
            {
                if (_trip == value) return;

                _trip = value;
                RaisePropertyChanged("Trip");
            }
        }

        public bool CanCancelCreateTrip
        {
            get { return Navigator.CanGoBack; }
        }

        public ICommand SaveTripCommand { get; private set; }

        public ICommand CancelCreateTripCommand { get; private set; }

        public override void OnNavigatedTo(Navigation.TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _tripRepository = RepositoryFactory.TripRepository;

            InitializeTrip();
        }

        private void InitializeTrip()
        {
            Trip = new Trip();
        }
    }
}

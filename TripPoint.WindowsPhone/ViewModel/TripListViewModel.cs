#region SDK Usings
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripListViewModel : TripPointViewModelBase
    {
        private ITripRepository _tripRepository;

        public TripListViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            _tripRepository = repositoryFactory.TripRepository;

            InitializeTrips();

            InitializeCommands();
        }

        private void InitializeTrips()
        {
            Trips = new ObservableCollection<Trip>(_tripRepository.Trips);
        }

        private void InitializeCommands()
        {
            ViewTripDetailsCommand = new RelayCommand<Trip>(ViewTripDetailsAction);
        }

        private void ViewTripDetailsAction(Trip trip)
        {
            Logger.Log(this, "View trip: {0}", trip);

            Navigator.Navigate(
                string.Format("/Trip/{0}/Details", trip.ID));
        }

        public ObservableCollection<Trip> Trips { get; private set; }

        public ICommand ViewTripDetailsCommand { get; private set; }
    }
}

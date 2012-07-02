#region SDK Usings
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripListViewModel : TripPointViewModelBase
    {
        private ITripRepository _tripRepository;

        public TripListViewModel(ITripRepository tripRepository)
        {
            if (tripRepository == null)
                throw new ArgumentNullException("tripRepository");

            _tripRepository = tripRepository;

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
        }

        public ObservableCollection<Trip> Trips { get; private set; }

        public ICommand ViewTripDetailsCommand { get; private set; }
    }
}

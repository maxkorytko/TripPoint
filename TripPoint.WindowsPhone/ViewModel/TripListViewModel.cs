#region SDK Usings
using System;
using System.Collections.ObjectModel;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;

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
        }

        private void InitializeTrips()
        {
            Trips = new ObservableCollection<Trip>(_tripRepository.Trips);
        }

        public ObservableCollection<Trip> Trips { get; private set; }
    }
}

#region SDK Usings
using System;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripDetailsViewModel : TripPointViewModelBase
    {
        private ITripRepository _tripRepository;

        public TripDetailsViewModel(ITripRepository tripRepository)
        {
            if (tripRepository == null)
                throw new ArgumentNullException("tripRepository");

            _tripRepository = tripRepository;

            InitializeTrip();
        }

        private void InitializeTrip()
        {
            Trip = new Trip();

            var tripID = GetTripID();

            var trip = _tripRepository.FindTrip(tripID);

            if (trip != null)
                Trip = trip;

            Logger.Log(this, "Trip has been initialized. {0}", Trip);
        }

        private static string GetTripID()
        {
            string tripID;

            var currentPage = TripPointNavigation.CurrentPage;

            if (currentPage != null)
                tripID = currentPage.TryGetQueryStringParameter("tripID");
            else
                tripID = string.Empty;

            return tripID;
        }

        public Trip Trip { get; private set; }
    }
}
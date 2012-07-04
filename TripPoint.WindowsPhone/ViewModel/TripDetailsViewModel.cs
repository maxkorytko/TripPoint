﻿#region SDK Usings
using System;
using Microsoft.Phone.Controls;
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

            Trip = new Trip();
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var tripID = GetTripID(e.View);
            
            var trip = GetTrip(tripID);

            if (trip != null)
                Trip = trip;

            Logger.Log(this, "Trip has been initialized. {0}", Trip);
        }

        private static string GetTripID(PhoneApplicationPage view)
        {
            string tripID = string.Empty;

            if (view != null)
                tripID = view.TryGetQueryStringParameter("tripID");
            
            return tripID;
        }

        private Trip GetTrip(string tripID)
        {
            return _tripRepository.FindTrip(tripID);
        }

        public Trip Trip { get; private set; }
    }
}
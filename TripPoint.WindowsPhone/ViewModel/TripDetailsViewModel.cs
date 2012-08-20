#region SDK Usings
using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.I18N;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripDetailsViewModel : TripPointViewModelBase
    {
        private Trip _trip;
        private ITripRepository _tripRepository;

        public TripDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            DeleteTripCommand = new RelayCommand(DeleteTripAction);
        }

        private void DeleteTripAction()
        {
            var userDecision = MessageBox.Show(Resources.ConfirmDeleteTrip, Resources.Deleting,
                MessageBoxButton.OKCancel);

            if (userDecision == MessageBoxResult.OK)
                DeleteTrip();
        }

        private void DeleteTrip()
        {
            if (_tripRepository != null)
            {
                _tripRepository.DeleteTrip(Trip);
            }

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

        public ICommand DeleteTripCommand { get; private set; }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _tripRepository = RepositoryFactory.TripRepository;

            var tripID = GetTripID(e.View);
            InitializeTrip(tripID);
        }

        private static int GetTripID(PhoneApplicationPage view)
        {
            var tripID = -1;

            if (view != null)
                tripID = Convert.ToInt32(view.TryGetQueryStringParameter("tripID"));
            
            return tripID;
        }

        private void InitializeTrip(int tripID)
        {
            if (_tripRepository == null) return;

            Trip = _tripRepository.FindTrip(tripID);
        }

        private Trip GetTrip(int tripID)
        {
            return _tripRepository.FindTrip(tripID);
        }
    }
}
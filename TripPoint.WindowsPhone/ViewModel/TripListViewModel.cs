using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.State;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripListViewModel : Base.TripPointViewModelBase
    {
        private static readonly string LAST_VIEWED_TRIP_ID = "LastViewedTripID";

        private ITripRepository _tripRepository;
        private ICollection<Trip> _trips;
        private bool _noCurrentTrip;
        private bool _noPastTrips;

        public TripListViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateTripCommand = new RelayCommand(CreateTripAction);
            ViewTripDetailsCommand = new RelayCommand<Trip>(ViewTripDetailsAction);
        }

        private void CreateTripAction()
        {
            Navigator.Navigate("/Trip/Create");
        }

        private void ViewTripDetailsAction(Trip trip)
        {
            if (trip == null) return;

            StateManager.Instance.Set<int>(LAST_VIEWED_TRIP_ID, trip.ID);
            Navigator.Navigate(string.Format("/Trip/{0}/Details", trip.ID));
        }

        public ICollection<Trip> Trips
        {
            get { return _trips; }
            set
            {
                if (_trips == value) return;

                _trips = value;
                RaisePropertyChanged("Trips");
            }
        }

        public bool NoCurrentTrip
        {
            get { return _noCurrentTrip; }
            set
            {
                if (_noCurrentTrip == value) return;

                _noCurrentTrip = value;
                RaisePropertyChanged("NoCurrentTrip");
            }
        }

        public bool NoPastTrips
        {
            get { return _noPastTrips; }
            set
            {
                if (_noPastTrips == value) return;

                _noPastTrips = value;
                RaisePropertyChanged("NoPastTrips");
            }
        }

        private IEnumerable<Trip> PastTrips
        {
            get
            {
                if (_tripRepository == null) return new List<Trip>();

                return (from trip in _tripRepository.Trips
                        where trip.EndDate.HasValue
                        select trip)
                        .OrderByDescending(trip => trip.EndDate);
            }
        }

        public ICommand CreateTripCommand { get; private set; }

        public ICommand ViewTripDetailsCommand { get; private set; }

        public override void OnNavigatedTo(Navigation.TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _tripRepository = RepositoryFactory.TripRepository;

            ResetViewModel();
            InitializeTrips();
            RefreshTrips();
            StateManager.Instance.Remove(LAST_VIEWED_TRIP_ID);
        }

        private void ResetViewModel()
        {
            NoCurrentTrip = _tripRepository.CurrentTrip == null;
            NoPastTrips = PastTrips.Count() == 0;
        }

        private void InitializeTrips()
        {
            if (_tripRepository == null) return;
            if (Trips != null) return;

            Trips = new ObservableCollection<Trip>(PastTrips);
        }

        private void RefreshTrips()
        {
            if (Trips == null) return;

            if (Trips.Count != PastTrips.Count())
            {
                Trips = null;
                InitializeTrips();
                return;
            }

            RefreshLastViewedTrip();
        }

        private void RefreshLastViewedTrip()
        {
            if (!StateManager.Instance.Contains(LAST_VIEWED_TRIP_ID)) return;

            var id = StateManager.Instance.Get<int>(LAST_VIEWED_TRIP_ID);

            var staleTrip = Trips.FirstOrDefault(trip => trip.ID == id);
            var updatedTrip = PastTrips.FirstOrDefault(trip => trip.ID == id);

            RefreshTrip(staleTrip, updatedTrip);
        }

        private void RefreshTrip(Trip target, Trip source)
        {
            if (target == null || source == null) return;

            target.Name = source.Name;
            target.Notes = source.Notes;
        }
    }
}

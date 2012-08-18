#region SDK Usings
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
#endregion

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripListViewModel : TripPointViewModelBase
    {
        private ITripRepository _tripRepository;
        private bool _noCurrentTrip;

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
            Navigator.Navigate(string.Format("/Trip/{0}/Details", trip.ID));
        }

        public ObservableCollection<Trip> Trips { get; private set; }

        /// <summary>
        /// Flag property for hiding/showing the application bar
        /// </summary>
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

        public ICommand CreateTripCommand { get; private set; }

        public ICommand ViewTripDetailsCommand { get; private set; }

        public override void OnNavigatedTo(Navigation.TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _tripRepository = RepositoryFactory.TripRepository;

            InitializeTrips();
            InitializeNoCurrentTrip();
        }

        private void InitializeTrips()
        {
            if (_tripRepository == null) return;

            var trips = (from trip in _tripRepository.Trips
                         where trip.EndDate.HasValue
                         select trip)
                        .OrderByDescending(t => t.EndDate);

            Trips = new ObservableCollection<Trip>(trips);
        }

        private void InitializeNoCurrentTrip()
        {
            if (_tripRepository == null) return;

            NoCurrentTrip = _tripRepository.CurrentTrip == null;
        }
    }
}

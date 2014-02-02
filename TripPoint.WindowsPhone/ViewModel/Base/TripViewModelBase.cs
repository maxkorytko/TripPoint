using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone.ViewModel.Base
{
    /// <summary>
    /// Encapsulates the behavior to retrieve a trip from the repository and keep it updated
    /// </summary>
    public abstract class TripViewModelBase : TripPointViewModelBase
    {
        private Trip _trip;
        
        public TripViewModelBase(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        { 
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

        protected ITripRepository TripRepository { get; private set; }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            InitializeTrip(TripPointConvert.ToInt32(GetParameter(e.View, "tripID")));
        }

        private void InitializeTrip(int tripID)
        {
            TripRepository = RepositoryFactory.TripRepository;
            Trip = GetTrip(tripID);
        }

        /// <summary>
        /// Retrieves and returns the trip with the given ID or an emty trip
        /// </summary>
        /// <param name="tripID"></param>
        /// <returns></returns>
        protected virtual Trip GetTrip(int tripID)
        {
            return TripRepository.FindTrip(tripID) ?? new Trip();
        }

        public override void OnBackNavigatedTo()
        {
            base.OnBackNavigatedTo();

            // it's important to refresh the trip when the user navigates back to the view,
            // in order to ensure the trip has the latest changes made to it from other views
            //
            RefreshTrip();
        }

        private void RefreshTrip()
        {
            if (Trip == null) return;

            InitializeTrip(Trip.ID);
        }

        /// <summary>
        /// Resets view model's state to initial values
        /// </summary>
        public virtual void ResetViewModel()
        {
            Trip = null;
            TripRepository = null;
        }
    }
}

using TripPoint.Model.Data;
using TripPoint.Model.Data.Repository;
using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class ViewModelLocator
    {
        private static TripListViewModel _tripListViewModel;
        private static CreateTripViewModel _createTripViewModel;
        private static CurrentTripViewModel _currentTripViewModel;
        private static TripDetailsViewModel _tripDetailsViewModel;
        private static CreateCheckpointViewModel _createCheckpointViewModel;
        private static CheckpointAddNotesViewModel _checkpointAddNotesViewModel;

        /// <summary>
        /// Initializes a new instance of the GlobalViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            InitializeTripListViewModel();
            InitializeCreateTripViewModel();
            InitializeCurrentTripViewModel();
            InitializeTripDetailsViewModel();
            InitializeCreateCheckpointViewModel();
            InitializeCheckpointAddNotesViewModel();
        }

        #region TripListViewModel
        private static void InitializeTripListViewModel()
        {
            if (_tripListViewModel == null)
                _tripListViewModel = new TripListViewModel(TripRepository);
        }

        public static TripListViewModel TripListViewModelStatic
        {
            get
            {
                InitializeTripListViewModel();
                return _tripListViewModel;
            }
        }

        public TripListViewModel TripListViewModel
        {
            get
            {
                return TripListViewModelStatic;
            }
        }

        private static void ClearTripListViewModel()
        {
            _tripListViewModel.Cleanup();
            _tripListViewModel = null;
        }
        #endregion

        #region CreateTripViewModel
        private static void InitializeCreateTripViewModel()
        {
            if (_createTripViewModel == null)
                _createTripViewModel = new CreateTripViewModel(TripRepository);
        }

        public static CreateTripViewModel CreateTripViewModelStatic
        {
            get
            {
                InitializeCreateTripViewModel();
                return _createTripViewModel;
            }
        }

        public CreateTripViewModel CreateTripViewModel
        {
            get
            {
                return CreateTripViewModelStatic;
            }
        }

        private static void ClearCreateTripViewModel()
        {
            _createTripViewModel.Cleanup();
            _createTripViewModel = null;
        }
        #endregion

        #region CurrentTripViewModel
        private static void InitializeCurrentTripViewModel()
        {
            if (_currentTripViewModel == null)
                _currentTripViewModel = new CurrentTripViewModel();
        }

        public static CurrentTripViewModel CurrentTripViewModelStatic
        {
            get
            {
                InitializeCurrentTripViewModel();
                return _currentTripViewModel;
            }
        }

        public CurrentTripViewModel CurrentTripViewModel
        {
            get
            {
                return CurrentTripViewModelStatic;
            }
        }

        private static void ClearCurrentTripViewModel()
        {
            _currentTripViewModel.Cleanup();
            _currentTripViewModel = null;
        }
        #endregion

        #region TripDetailsViewModel
        private static void InitializeTripDetailsViewModel()
        {
            if (_tripDetailsViewModel == null)
                _tripDetailsViewModel = new TripDetailsViewModel(TripRepository);
        }

        public static TripDetailsViewModel TripDetailsViewModelStatic
        {
            get
            {
                InitializeTripDetailsViewModel();
                return _tripDetailsViewModel;
            }
        }

        public TripDetailsViewModel TripDetailsViewModel
        {
            get
            {
                return TripDetailsViewModelStatic;
            }
        }

        private static void ClearTripDetailsViewModel()
        {
            _tripDetailsViewModel.Cleanup();
            _tripDetailsViewModel = null;
        }
        #endregion

        #region CreateCheckpointViewModel
        private static void InitializeCreateCheckpointViewModel()
        {
            if (_createCheckpointViewModel == null)
                _createCheckpointViewModel = new CreateCheckpointViewModel(TripRepository);
        }

        public static CreateCheckpointViewModel CreateCheckpointViewModelStatic
        {
            get
            {
                InitializeCreateCheckpointViewModel();
                return _createCheckpointViewModel;
            }
        }

        public CreateCheckpointViewModel CreateCheckpointViewModel
        {
            get
            {
                return CreateCheckpointViewModelStatic;
            }
        }

        private static void ClearCreateCheckpointViewModel()
        {
            _createCheckpointViewModel.Cleanup();
            _createCheckpointViewModel = null;
        }
        #endregion

        #region CheckpointAddNotesViewModel
        private static void InitializeCheckpointAddNotesViewModel()
        {
            if (_checkpointAddNotesViewModel == null)
                _checkpointAddNotesViewModel = new CheckpointAddNotesViewModel(TripRepository);
        }

        public static CheckpointAddNotesViewModel CheckpointAddNotesViewModelStatic
        {
            get
            {
                InitializeCheckpointAddNotesViewModel();
                return _checkpointAddNotesViewModel;
            }
        }

        public CheckpointAddNotesViewModel CheckpointAddNotesViewModel
        {
            get
            {
                return CheckpointAddNotesViewModelStatic;
            }
        }

        private static void ClearCheckpointAddNotesViewModel()
        {
            _checkpointAddNotesViewModel.Cleanup();
            _checkpointAddNotesViewModel = null;
        }
        #endregion

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearCreateTripViewModel();
            ClearCurrentTripViewModel();
            ClearCreateCheckpointViewModel();
            ClearCheckpointAddNotesViewModel();
        }

        private static ITripRepository TripRepository
        {
            get
            {
                var dataContext = new TripPointDataContext(TripPointDataContext.ConnectionString);
                //dataContext.Log = new TripPoint.Model.Utils.DebugStreamWriter();
                return new DatabaseTripRepository(dataContext);
            }
        }
    }
}
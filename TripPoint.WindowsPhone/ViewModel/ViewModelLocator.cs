using TripPoint.Model.Data;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
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
        private static CheckpointEditViewModel _checkpointEditViewModel;
        private static CheckpointAddNotesViewModel _checkpointAddNotesViewModel;
        private static CheckpointAddPicturesViewModel _checkpointAddPicturesViewModel;
        private static CheckpointDetailsViewModel _checkpointDetailsViewModel;
        private static PictureDetailsViewModel _pictureDetailsViewModel;
        private static ApplicationSettingsViewModel _applicationSettingsViewModel;

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
            InitializeCheckpointAddPicturesViewModel();
            InitializeCheckpointDetailsViewModel();
            InitializePictureDetailsViewModel();
            InitializeApplicationSettingsViewModel();
        }

        #region TripListViewModel
        private static void InitializeTripListViewModel()
        {
            if (_tripListViewModel == null)
                _tripListViewModel = new TripListViewModel(RepositoryFactory.Create());
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
                _createTripViewModel = new CreateTripViewModel(RepositoryFactory.Create());
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
            {   
                _currentTripViewModel = new CurrentTripViewModel(RepositoryFactory.Create());
            }
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
                _tripDetailsViewModel = new TripDetailsViewModel(RepositoryFactory.Create());
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
                _createCheckpointViewModel = new CreateCheckpointViewModel(RepositoryFactory.Create());
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

        #region CheckpointEditViewModel
        private static void InitializeCheckpointEditViewModel()
        {
            if (_checkpointEditViewModel == null)
                _checkpointEditViewModel = new CheckpointEditViewModel(RepositoryFactory.Create());
        }

        public static CheckpointEditViewModel CheckpointEditViewModelStatic
        {
            get
            {
                InitializeCheckpointEditViewModel();
                return _checkpointEditViewModel;
            }
        }

        public CheckpointEditViewModel CheckpointEditViewModel
        {
            get
            {
                return CheckpointEditViewModelStatic;
            }
        }

        private static void ClearCheckpointEditViewModel()
        {
            _checkpointEditViewModel.Cleanup();
            _checkpointEditViewModel = null;
        }
        #endregion

        #region CheckpointAddNotesViewModel
        private static void InitializeCheckpointAddNotesViewModel()
        {
            if (_checkpointAddNotesViewModel == null)
                _checkpointAddNotesViewModel = new CheckpointAddNotesViewModel(RepositoryFactory.Create());
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

        #region CheckpointAddPicturesViewModel
        private static void InitializeCheckpointAddPicturesViewModel()
        {
            if (_checkpointAddPicturesViewModel == null)
                _checkpointAddPicturesViewModel = new CheckpointAddPicturesViewModel(RepositoryFactory.Create());
        }

        public static CheckpointAddPicturesViewModel CheckpointAddPicturesViewModelStatic
        {
            get
            {
                InitializeCheckpointAddPicturesViewModel();
                return _checkpointAddPicturesViewModel;
            }
        }

        public CheckpointAddPicturesViewModel CheckpointAddPicturesViewModel
        {
            get
            {
                return CheckpointAddPicturesViewModelStatic;
            }
        }

        private static void ClearCheckpointAddPicturesViewModel()
        {
            _checkpointAddPicturesViewModel.Cleanup();
            _checkpointAddPicturesViewModel = null;
        }
        #endregion

        #region CheckpointDetailsViewModel
        private static void InitializeCheckpointDetailsViewModel()
        {
            if (_checkpointDetailsViewModel == null)
                _checkpointDetailsViewModel = new CheckpointDetailsViewModel(RepositoryFactory.Create());
        }

        public static CheckpointDetailsViewModel CheckpointDetailsViewModelStatic
        {
            get
            {
                InitializeCheckpointDetailsViewModel();
                return _checkpointDetailsViewModel;
            }
        }

        public CheckpointDetailsViewModel CheckpointDetailsViewModel
        {
            get
            {
                return CheckpointDetailsViewModelStatic;
            }
        }

        private static void ClearCheckpointDetailsViewModel()
        {
            _checkpointDetailsViewModel.Cleanup();
            _checkpointDetailsViewModel = null;
        }
        #endregion

        #region PictureDetailsViewModel
        private static void InitializePictureDetailsViewModel()
        {
            if (_pictureDetailsViewModel == null)
                _pictureDetailsViewModel = new PictureDetailsViewModel(RepositoryFactory.Create());
        }

        public static PictureDetailsViewModel PictureDetailsViewModelStatic
        {
            get
            {
                InitializePictureDetailsViewModel();
                return _pictureDetailsViewModel;
            }
        }

        public PictureDetailsViewModel PictureDetailsViewModel
        {
            get
            {
                return PictureDetailsViewModelStatic;
            }
        }

        private static void ClearPictureDetailsViewModel()
        {
            _pictureDetailsViewModel.Cleanup();
            _pictureDetailsViewModel = null;
        }
        #endregion

        #region ApplicationSettingsViewModel
        private static void InitializeApplicationSettingsViewModel()
        {
            if (_applicationSettingsViewModel == null)
                _applicationSettingsViewModel = new ApplicationSettingsViewModel();
        }

        public static ApplicationSettingsViewModel ApplicationSettingsViewModelStatic
        {
            get
            {
                InitializeApplicationSettingsViewModel();
                return _applicationSettingsViewModel;
            }
        }

        public ApplicationSettingsViewModel ApplicationSettingsViewModel
        {
            get
            {
                return ApplicationSettingsViewModelStatic;
            }
        }

        private static void ClearApplicationSettingsViewModel()
        {
            _applicationSettingsViewModel.Cleanup();
            _applicationSettingsViewModel = null;
        }
        #endregion
        
        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearTripListViewModel();
            ClearCreateTripViewModel();
            ClearCurrentTripViewModel();
            ClearTripDetailsViewModel();
            ClearCreateCheckpointViewModel();
            ClearCheckpointEditViewModel();
            ClearCheckpointAddNotesViewModel();
            ClearCheckpointAddPicturesViewModel();
            ClearCheckpointDetailsViewModel();
            ClearPictureDetailsViewModel();
            ClearApplicationSettingsViewModel();
        }
    }
}
using TripPoint.Model.Data.Repository.Memory;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class ViewModelLocator
    {
        private static CreateTripViewModel _createTripViewModel;
        private static CurrentTripViewModel _currentTripViewModel;

        /// <summary>
        /// Initializes a new instance of the GlobalViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            InitializeCreateTripViewModel();
            InitializeCurrentTripViewModel();
        }

        #region CreateTripViewModel
        private static void InitializeCreateTripViewModel()
        {
            if (_createTripViewModel == null)
                _createTripViewModel = new CreateTripViewModel(new MemoryTripRepository());
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

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearCreateTripViewModel();
            ClearCurrentTripViewModel();
        }
    }
}
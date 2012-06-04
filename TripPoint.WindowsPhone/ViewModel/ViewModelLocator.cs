namespace TripPoint.WindowsPhone.ViewModel
{
    public class ViewModelLocator
    {
        private static CreateTripViewModel _createTripViewModel;

        /// <summary>
        /// Initializes a new instance of the GlobalViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            InitCreateTripViewModel();
        }

        public static CreateTripViewModel CreateTripViewModel
        {
            get
            {
                InitCreateTripViewModel();
                return _createTripViewModel;
            }
        }

        private static void InitCreateTripViewModel()
        {
            if (_createTripViewModel == null)
                _createTripViewModel = new CreateTripViewModel();
        }

        private static void ClearCreateTripViewModel()
        {
            _createTripViewModel.Cleanup();
            _createTripViewModel = null;
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearCreateTripViewModel();
        }
    }
}
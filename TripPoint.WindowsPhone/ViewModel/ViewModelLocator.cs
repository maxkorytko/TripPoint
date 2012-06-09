using TripPoint.Model.Data.Repository.Memory;

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

        private static void InitCreateTripViewModel()
        {
            if (_createTripViewModel == null)
                _createTripViewModel = new CreateTripViewModel(new MemoryTripRepository());
        }

        public static CreateTripViewModel CreateTripViewModelStatic
        {
            get
            {
                InitCreateTripViewModel();
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

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearCreateTripViewModel();
        }
    }
}
using TripPoint.WindowsPhone.I18N;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight;

namespace TripPoint.WindowsPhone.ViewModel
{
    /// <summary>
    /// Base class for application view models
    /// </summary>
    public abstract class TripPointViewModelBase : ViewModelBase
    {
        private Localization _localization = new Localization();

        public Localization Localization
        {
            get 
            { 
                return _localization; 
            }
        }

        /// <summary>
        /// Navigation service allowing view models to navigate between pages
        /// </summary>
        protected INavigator Navigator
        {
            get { return TripPointNavigation.Navigator; }
        }

        /// <summary>
        /// Called when a view managed by this view model has been navigated to
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnNavigatedTo(TripPointNavigationEventArgs e) { }
    }
}

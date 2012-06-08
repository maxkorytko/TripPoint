
using GalaSoft.MvvmLight;
using TripPoint.WindowsPhone.I18N;

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
    }
}

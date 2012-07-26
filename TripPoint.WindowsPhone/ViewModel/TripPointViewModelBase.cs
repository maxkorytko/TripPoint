using System;

using TripPoint.Model.Data.Repository.Factory;
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
        private IRepositoryFactory _repositoryFactory = new NullRepositoryFactory();

        public Localization Localization
        {
            get 
            { 
                return _localization; 
            }
        }

        public IRepositoryFactory RepositoryFactory
        {
            get { return _repositoryFactory; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _repositoryFactory = value;
            }
        }

        /// <summary>
        /// Called when a view managed by this view model has been navigated to
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnNavigatedTo(TripPointNavigationEventArgs e) { }
    }
}

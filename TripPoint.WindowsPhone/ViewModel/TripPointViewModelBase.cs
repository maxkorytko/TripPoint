using System;
using System.Windows;
using Microsoft.Phone.Controls;

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
        private IRepositoryFactory _repositoryFactory;

        private Localization _localization = new Localization();

        public TripPointViewModelBase(IRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");

            _repositoryFactory = repositoryFactory;
        }

        public Localization Localization
        {
            get { return _localization; }
        }

        /// <summary>
        /// The factory for creating repositories
        /// Subclasses can create any kind of repository using the factory
        /// </summary>
        public IRepositoryFactory RepositoryFactory
        {
            get { return _repositoryFactory; }
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

        public bool IsViewTopMost
        {
            get
            {
                Object dataContext = null;

                var content = (Application.Current as App).RootFrame.Content;
                
                if (content is PhoneApplicationPage)
                    dataContext = (content as PhoneApplicationPage).DataContext;

                return dataContext == this;
            }
        }
    }
}

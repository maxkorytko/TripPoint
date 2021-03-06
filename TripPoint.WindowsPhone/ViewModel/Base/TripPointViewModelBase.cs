﻿using System;
using System.Windows;
using Microsoft.Phone.Controls;

using TripPoint.I18N;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight;

namespace TripPoint.WindowsPhone.ViewModel.Base
{
    /// <summary>
    /// Base class for application view models
    /// </summary>
    public abstract class TripPointViewModelBase : ViewModelBase
    {
        private IRepositoryFactory _repositoryFactory;

        private Localization _localization = new Localization();

        public TripPointViewModelBase() { }

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

        /// <summary>
        /// Called when a view managed by this view model has been navigated to using back navigation
        /// Currently, it's up to the view to call this method from its OnNavigatedTo override,
        /// provided that NavigationMode is NavigationMode.Back
        /// </summary>
        public virtual void OnBackNavigatedTo() { }

        /// <summary>
        /// Determines if the view, which the view model is bound to, is on top of the navigation stack
        /// </summary>
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

        /// <summary>
        /// Helper method for extracting a query string parameter from the given view
        /// </summary>
        /// <param name="view"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static string GetParameter(PhoneApplicationPage view, string parameterName)
        {
            if (view == null) return String.Empty;

            return view.TryGetQueryStringParameter(parameterName);
        }
    }
}

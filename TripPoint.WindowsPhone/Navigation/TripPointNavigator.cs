using System;
using System.Linq;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.Navigation
{
    public class TripPointNavigator : INavigator
    {
        private PhoneApplicationFrame _rootFrame;
        
        private delegate void TripPointNavigatedEventHandler(NavigationEventArgs args);

        public TripPointNavigator(PhoneApplicationFrame rootFrame)
        {
            if (rootFrame == null)
                throw new ArgumentNullException("rootFrame");

            _rootFrame = rootFrame;
        }

        public void ClearHistory()
        {
            while (_rootFrame.BackStack.Count() > 0)
            {
                _rootFrame.RemoveBackEntry();
            }
        }

        public void GoBack()
        {
            PerformBackNavigation(args =>
            {
                TripPointNavigator.NotifyViewModelIfPossible(args);
            });
        }

        /// <summary>
        /// Moves back once in navigation history
        /// Fires the passed in event handler when navigation is completed
        /// </summary>
        /// <param name="eventHandler"></param>
        private void PerformBackNavigation(TripPointNavigatedEventHandler eventHandler)
        {
            if (!_rootFrame.CanGoBack) return;

            NavigatedEventHandler navigatedEventHandler = null;
            navigatedEventHandler = (sender, args) =>
            {
                _rootFrame.Navigated -= navigatedEventHandler;

                if (eventHandler != null)
                    eventHandler(args);
            };

            _rootFrame.Navigated += navigatedEventHandler;

            _rootFrame.GoBack();
        }

        public bool Navigate(string uri)
        {
            return PerformNavigation(uri, args =>
            {
                TripPointNavigator.NotifyViewModelIfPossible(args);
            });
        }

        public bool NavigateWithoutHistory(string uri)
        {
            return PerformNavigation(uri, args =>
            {
                RemoveBackStackEntry();
                TripPointNavigator.NotifyViewModelIfPossible(args);
            });
        }

        /// <summary>
        /// Navigates to a given URI
        /// Fires the passed in event handler when navigation is completed
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        private bool PerformNavigation(string uri, TripPointNavigatedEventHandler eventHandler)
        {
            if (string.IsNullOrWhiteSpace(uri)) return false;

            NavigatedEventHandler navigatedEventHandler = null;
            navigatedEventHandler = (sender, args) =>
            {
                _rootFrame.Navigated -= navigatedEventHandler;

                PrintBackStack();

                if (eventHandler != null)
                    eventHandler(args);
            };

            _rootFrame.Navigated += navigatedEventHandler;

            var success = _rootFrame.Navigate(new Uri(uri, UriKind.Relative));

            if (!success)
                _rootFrame.Navigated -= navigatedEventHandler;

            return success;
        }

        /// <summary>
        /// Notifies the view model of the navigation event
        /// If the data context of the navigated view is the view model, invokes the hook method
        /// </summary>
        /// <param name="e"></param>
        private static void NotifyViewModelIfPossible(NavigationEventArgs e)
        {
            PhoneApplicationPage page = null;

            if (!(e.Content is PhoneApplicationPage)) return;

            page = e.Content as PhoneApplicationPage;

            if (page.DataContext != null && (page.DataContext is TripPointViewModelBase))
            {
                var navigationEventArgs = new TripPointNavigationEventArgs(e, page);

                (page.DataContext as TripPointViewModelBase).OnNavigatedTo(navigationEventArgs);
            }
        }

        /// <summary>
        /// Removes one entry from the back navigation history
        /// </summary>
        private void RemoveBackStackEntry()
        {
            if (_rootFrame.BackStack.Count() > 0)
                _rootFrame.RemoveBackEntry();
        }

        private void PrintBackStack()
        {
            if (_rootFrame.BackStack == null) return;

            TripPoint.Model.Utils.Logger.Log("\n*****");

            foreach (var entry in _rootFrame.BackStack)
            {
                TripPoint.Model.Utils.Logger.Log(entry.Source.OriginalString);
            }

            TripPoint.Model.Utils.Logger.Log("*****\n");
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.Navigation
{
    public class TripPointNavigator : INavigator
    {
        private PhoneApplicationFrame _rootFrame;

        public TripPointNavigator()
        {
            _rootFrame = (Application.Current as App).RootFrame;

            if (_rootFrame == null)
                throw new InvalidOperationException("Application RootFrame is null");
        }

        public bool Navigate(string uri)
        {
            _rootFrame.Navigated += new NavigatedEventHandler(Navigated);

            return _rootFrame.Navigate(new Uri(uri, UriKind.Relative));
        }

        /// <summary>
        /// Listener for the Navigated event
        /// Passes navigation event handling to the view model of the page for which navigation event has been raised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Navigated(object sender, NavigationEventArgs e)
        {
            PrintBackStack();

            PhoneApplicationPage page = null;

            if (!(e.Content is PhoneApplicationPage)) return;

            page = e.Content as PhoneApplicationPage;

            if (page.DataContext != null && (page.DataContext is TripPointViewModelBase))
            {
                var navigationEventArgs = new TripPointNavigationEventArgs(e, page);

                (page.DataContext as TripPointViewModelBase).OnNavigatedTo(navigationEventArgs);
            }

            _rootFrame.Navigated -= Navigated;
        }

        // TODO: remove; this is here for debugging only
        private void PrintBackStack()
        {
            foreach (var entry in _rootFrame.BackStack)
            {
                TripPoint.Model.Utils.Logger.Log("Entry: " + entry.Source.OriginalString);
            }

            TripPoint.Model.Utils.Logger.Log("*****\n");
        }

        public void GoBack()
        {
            if (_rootFrame.CanGoBack)
                _rootFrame.GoBack();
        }
    }
}

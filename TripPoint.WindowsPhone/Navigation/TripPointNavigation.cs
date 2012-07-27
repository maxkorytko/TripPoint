#region SDK Usings

using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

#endregion

using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.Navigation
{
    public class TripPointNavigation
    {
        private static INavigator _navigator;

        public static void Initialize(INavigator navigator)
        {
            if (navigator == null)
                throw new ArgumentNullException("navigator");

            _navigator = navigator;
        }

        public static INavigator Navigator
        {
            get { return _navigator; }
        }

        /// <summary>
        /// Listener for the Navigated event
        /// Passes navigation event handling to the view model of the page for which navigation event has been raised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Navigated(Object sender, NavigationEventArgs e)
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
    }
}

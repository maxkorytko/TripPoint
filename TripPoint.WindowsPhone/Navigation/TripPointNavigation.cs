#region SDK Usings

using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

#endregion

using TripPoint.Model.Utils;
using TripPoint.WindowsPhone;
using TripPoint.WindowsPhone.ViewModel;

namespace TripPoint.WindowsPhone.Navigation
{
    /// <summary>
    /// Navigation facade for the application
    /// </summary>
    public class TripPointNavigation
    {
        /// <summary>
        /// Navigates to a previous page if possible
        /// </summary>
        public static void GoBack()
        {
            if ((Application.Current as App).RootFrame.CanGoBack)
                (Application.Current as App).RootFrame.GoBack();
        }

        /// <summary>
        /// Navigates to a given URI
        /// </summary>
        /// <param name="uri">URI to navigate to</param>
        /// <returns>true if navigation is successful</returns>
        public static bool Navigate(string uri)
        {
            var rootFrame = (Application.Current as App).RootFrame;

            return rootFrame.Navigate(new Uri(uri, UriKind.Relative));
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

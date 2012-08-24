using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TripPoint.WindowsPhone.Navigation
{
    /// <summary>
    /// Navigation facade for the application
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Navigates to a given URI
        /// </summary>
        /// <param name="uri">URI to navigate to</param>
        /// <returns>true if navigation is successful</returns>
        bool Navigate(string uri);

        /// <summary>
        /// Navigates to a given URI
        /// Removes the page that called this method from the navigation history
        /// The user will not be able to go back to the page using the back button
        /// </summary>
        /// <param name="uri">URI to navigate to</param>
        /// <returns>true if navigation is successful</returns>
        bool NavigateWithoutHistory(string uri);

        /// <summary>
        /// Navigates to a previous page in the navigation history
        /// Does nothing if navigation history is empty
        /// </summary>
        void GoBack();
    }
}

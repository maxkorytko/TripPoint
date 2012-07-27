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
        /// Navigates to a previous page if possible
        /// </summary>
        void GoBack();
    }
}

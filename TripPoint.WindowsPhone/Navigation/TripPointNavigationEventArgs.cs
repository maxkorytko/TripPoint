#region SDK Usings
using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
#endregion

namespace TripPoint.WindowsPhone.Navigation
{
    /// <summary>
    /// A wrapper for NavigationEventArgs
    /// Exposes the page navigated to/from as a property
    /// </summary>
    public class TripPointNavigationEventArgs
    {
        public TripPointNavigationEventArgs(NavigationEventArgs navigationEventArgs)
            : this(navigationEventArgs, null)
        {   
        }

        public TripPointNavigationEventArgs(NavigationEventArgs navigationEventArgs,
            PhoneApplicationPage view)
        {
            if (navigationEventArgs == null)
                throw new ArgumentNullException("navigationEventArgs");

            NavigationEventArgs = navigationEventArgs;

            View = view;
        }

        public NavigationEventArgs NavigationEventArgs { get; private set; }

        public PhoneApplicationPage View { get; private set; }
    }
}

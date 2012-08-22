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
    }
}

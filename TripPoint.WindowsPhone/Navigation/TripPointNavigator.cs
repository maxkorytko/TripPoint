using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace TripPoint.WindowsPhone.Navigation
{
    public class TripPointNavigator : INavigator
    {
        PhoneApplicationFrame _rootFrame;

        public TripPointNavigator()
        {
            _rootFrame = (Application.Current as App).RootFrame;

            if (_rootFrame == null)
                throw new InvalidOperationException("Application RootFrame is null");
        }

        public bool Navigate(string uri)
        {
            return _rootFrame.Navigate(new Uri(uri, UriKind.Relative));
        }

        public void GoBack()
        {
            if (_rootFrame.CanGoBack)
                _rootFrame.GoBack();
        }
    }
}

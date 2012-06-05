using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TripPoint.WindowsPhone.I18N
{
    public class Localization
    {
        private static Resources _resources = new Resources();

        public Resources Resources
        {
            get
            {
                return _resources;
            }
        }
    }
}

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

using GalaSoft.MvvmLight;
using TripPoint.WindowsPhone.I18N;

namespace TripPoint.WindowsPhone.ViewModel
{
    /// <summary>
    /// Base class for application view models
    /// </summary>
    public class TripPointViewModelBase : ViewModelBase
    {
        private Localization _localization = new Localization();

        public Localization Localization
        {
            get 
            { 
                return _localization; 
            }
        }
    }
}

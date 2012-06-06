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

using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateTripViewModel : TripPointViewModelBase
    {
        public CreateTripViewModel()
        {
            SaveTripCommand = new RelayCommand(SaveTrip);
        }

        public RelayCommand SaveTripCommand
        {
            get;
            private set;
        }

        private void SaveTrip()
        {
            MessageBox.Show("Save Trip");
        }
    }
}

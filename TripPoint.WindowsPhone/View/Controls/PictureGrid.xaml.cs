using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TripPoint.Model.Utils;

namespace TripPoint.WindowsPhone.View.Controls
{
    public partial class PictureGrid : UserControl
    {
        public PictureGrid()
        {
            InitializeComponent();
        }

        private void PictureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox.SelectedItem == null) return;

            //(DataContext as TripListViewModel).ViewTripDetailsCommand.Execute(listBox.SelectedItem);

            Logger.Log(string.Format("Selected item index: {0}", listBox.SelectedIndex));

            listBox.SelectedItem = null;
        }
    }
}

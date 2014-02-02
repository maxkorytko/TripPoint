using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Device.Location;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.WindowsPhone.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using TripPoint.WindowsPhone.Utils.View;

namespace TripPoint.WindowsPhone.View.Checkpoint
{
    public partial class CheckpointDetailsView : PhoneApplicationPage
    {
        public CheckpointDetailsView()
        {
            InitializeComponent();
            
            RegisterMessages();
            
            Unloaded += (sender, args) => OnUnloaded();
        }

        private CheckpointDetailsViewModel ViewModel
        {
            get { return DataContext as CheckpointDetailsViewModel; }
        }

        private bool ShouldFreeUpResources { get; set; }

        private void RegisterMessages()
        {
            Messenger.Default.Register<PropertyChangedMessage<bool>>(this, message =>
            {
                if (message.PropertyName.Equals("ShouldShowCheckpointMap"))
                {
                    UpdateCheckpointMapVisibility(message.NewValue);
                }
            });
        }

        private void UpdateCheckpointMapVisibility(bool isVisible)
        {
            if (isVisible)
                ShowCheckpointMap();
            else
                HideCheckpointMap();
        }

        private void ShowCheckpointMap()
        {
            var mapCenter = ViewModel.Checkpoint.Location.GeoCoordinate ?? GeoCoordinate.Unknown;

            // we don't want to display the map unless we known the checkpoint location
            if (mapCenter == GeoCoordinate.Unknown) return;

            CheckpointMap.MapCenter = mapCenter;
            ShowPivotItem(CheckpointMapPivotItem);
        }

        private void ShowPivotItem(PivotItem item)
        {
            if (item == null) return;

            if (CheckpointDetails.Items.IndexOf(item) == -1)
                CheckpointDetails.Items.Add(item);
        }

        private void HideCheckpointMap()
        {
            CheckpointMap.MapCenter = GeoCoordinate.Unknown;
            HidePivotItem(CheckpointMapPivotItem);
        }

        private void HidePivotItem(PivotItem item)
        {
            if (item == null) return;

            CheckpointDetails.Items.Remove(item);
        }

        private void OnUnloaded()
        {
            Messenger.Default.Unregister(this);

            // we are releasing resources here, because the page is no longer visible at this point
            // since we're changing properties bound to UI controls,
            // it will help us avoid updating the UI while the page is visible as the use navigates back
            // (e.g.) picture count droppoing to 0, or replacing thumbnails grid with 'none'
            //
            if (ShouldFreeUpResources)
            {   
                ViewModel.ResetViewModel();
                PictureBitmapCache.Instance.Clear();
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (!ViewModel.IsNotesSelectionEnabled) return;
            
            ViewModel.IsNotesSelectionEnabled = false;
            e.Cancel = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.Back) ViewModel.OnBackNavigatedTo();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            ShouldFreeUpResources = e.NavigationMode == NavigationMode.Back;
        }

        private void NoteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = (sender as MultiselectList).SelectedItems;

            if (selectedItems != null)
                DeleteNotesActionBarButton.IsEnabled = selectedItems.Count > 0;
        }

        private void DeleteNotesActionBarButton_Click(object sender, EventArgs e)
        {
            if  (NoteList.SelectedItems == null) return;

            IList<Note> notesToDelete = new List<Note>();
            
            foreach (var item in NoteList.SelectedItems)
            {
                notesToDelete.Add(item as Note);
            }

            ViewModel.DeleteNotesCommand.Execute(notesToDelete);
        }
    }
}
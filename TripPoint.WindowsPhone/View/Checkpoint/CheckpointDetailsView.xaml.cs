using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Device.Location;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.WindowsPhone.State.Data;
using TripPoint.WindowsPhone.ViewModel;
using TripPoint.WindowsPhone.View.Controls;
using GalaSoft.MvvmLight.Messaging;

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

        private void RegisterMessages()
        {
            Messenger.Default.Register<PropertyChangedMessage<bool>>(this, message =>
            {
                if (message.PropertyName.Equals("ShouldShowCheckpointMap"))
                {
                    UpdateCheckpointMapVisibility(message.NewValue);
                }
                else if (message.PropertyName.Equals("IsNotesSelectionEnabled"))
                {
                    UpdateNoteListSelectionEnabled(message.NewValue);
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

        private void UpdateNoteListSelectionEnabled(bool isSelectionEnabled)
        {
            NoteList.IsSelectionEnabled = isSelectionEnabled;
        }

        private void OnUnloaded()
        {
            Messenger.Default.Unregister(this);
            ViewModel.ResetViewModel();
        }

        private void PictureSelected(object sender, PictureSelectedEventArgs args)
        {
            var selected = (args.SelectedPicture as PictureThumbnail);

            if (selected != null)
                ViewModel.ViewPictureCommand.Execute(selected.Picture);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (ViewModel.IsNotesSelectionEnabled)
            {
                ViewModel.IsNotesSelectionEnabled = false;
                e.Cancel = true;
                return;
            }

            // clear thumbnails to free up memory
            ViewModel.Thumbnails.Clear();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.OnNavigatedTo(new Navigation.TripPointNavigationEventArgs(e,
                e.Content as PhoneApplicationPage));
        }

        private void NoteList_IsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isSelectionEnabled = (sender as MultiselectList).IsSelectionEnabled;

            if (ViewModel.IsNotesSelectionEnabled != isSelectionEnabled)
                ViewModel.IsNotesSelectionEnabled = isSelectionEnabled;
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
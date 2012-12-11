using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.State.Data;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.I18N;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointDetailsViewModel : TripPointViewModelBase
    {
        Checkpoint _checkpoint;
        ICollection<Thumbnail> _thumbnails;
        bool _shouldShowCheckpointMap;
        ICheckpointRepository _checkpointRepository;
        IPictureRepository _pictureRepository;
        
        public CheckpointDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            Checkpoint = new Checkpoint();
            ShouldShowCheckpointMap = false;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditCheckpointCommand = new RelayCommand(EditCheckpointAction);
            DeleteCheckpointCommand = new RelayCommand(DeleteCheckpointAction);
            ViewPictureCommand = new RelayCommand<Picture>(ViewPictureAction);
        }

        public Checkpoint Checkpoint 
        {
            get { return _checkpoint; }
            private set
            {
                if (_checkpoint == value) return;

                _checkpoint = value;
                RaisePropertyChanged("Checkpoint");
            }
        }

        public ICollection<Thumbnail> Thumbnails
        {
            get
            {
                InitializeThumbnails();
                return _thumbnails;
            }
            private set
            {
                if (_thumbnails == value) return;

                _thumbnails = value;
            }
        }

        private void InitializeThumbnails()
        {
            if (_thumbnails != null) return;

            if (Checkpoint == null) return;

            _thumbnails = new List<Thumbnail>();

            foreach (var picture in Checkpoint.Pictures)
            {
                var thumbnail = new Thumbnail(picture);
                _thumbnails.Add(thumbnail);
            }
        }

        public bool ShouldShowCheckpointMap
        {
            get { return _shouldShowCheckpointMap; }
            set
            {
                // not checking if value has been updated
                // this is to ensure the map can be hidden by default

                var oldValue = _shouldShowCheckpointMap;
                _shouldShowCheckpointMap = value;

                // broadcast property changed message
                // the message will be caught in the code behind in order to update the UI
                //
                RaisePropertyChanged("ShouldShowCheckpointMap", oldValue, value, true);
            }
        }

        public ICommand EditCheckpointCommand { get; private set; }

        public ICommand DeleteCheckpointCommand { get; private set; }

        public ICommand ViewPictureCommand { get; private set; }

        private void EditCheckpointAction()
        {
            Logger.Log(this, "Edit checkpoint");
        }

        private void DeleteCheckpointAction()
        {
            var userDecision = MessageBox.Show(Resources.ConfirmDeleteCheckpoint, Resources.Deleting,
                MessageBoxButton.OKCancel);

            if (userDecision == MessageBoxResult.OK)
                DeleteCheckpoint();
        }

        private void DeleteCheckpoint()
        {
            _checkpointRepository.DeleteCheckpoint(Checkpoint);
            
            Navigator.GoBack();
        }

        private void ViewPictureAction(Picture picture)
        {
            if (Checkpoint == null || picture == null) return;

            Navigator.Navigate(String.Format("/Checkpoints/{0}/Pictures/{1}/Details", Checkpoint.ID,
                picture.ID));
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _checkpointRepository = RepositoryFactory.CheckpointRepository;
            _pictureRepository = RepositoryFactory.PictureRepository;

            var checkpointID = GetCheckpointID(e.View);
            InitializeCheckpoint(checkpointID);
            DetermineCheckpointMapAvailability();
        }

        private static int GetCheckpointID(PhoneApplicationPage view)
        {
            if (view == null) return -1;

            var parameter = view.TryGetQueryStringParameter("checkpointID");

            return TripPointConvert.ToInt32(parameter);
        }

        private void InitializeCheckpoint(int checkpointID)
        {
            var checkpoint = _checkpointRepository.FindCheckpoint(checkpointID);

            if (checkpoint != null)
                Checkpoint = checkpoint;
        }

        private void DetermineCheckpointMapAvailability()
        {
            // display the map provided that checkpoint location is available
            ShouldShowCheckpointMap = Checkpoint.Location != null && !Checkpoint.Location.IsUnknown;
        }

        public void ResetViewModel()
        {
            ShouldShowCheckpointMap = false;
            Thumbnails = null;
        }
    }
}

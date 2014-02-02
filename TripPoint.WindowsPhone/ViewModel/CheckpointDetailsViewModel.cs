using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.I18N;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointDetailsViewModel : Base.TripPointViewModelBase
    {
        private static readonly ImageSource PICTURE_THUMBNAIL_PLACEHOLDER =
            new BitmapImage(new Uri("", UriKind.RelativeOrAbsolute));

        private Checkpoint _checkpoint;
        private ICollection<Thumbnail> _thumbnails;
        private bool _shouldShowCheckpointMap;
        private bool _isNotesSelectionEnabled;
        private ICheckpointRepository _checkpointRepository;
        
        public CheckpointDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditCheckpointCommand = new RelayCommand(EditCheckpointAction);
            DeleteCheckpointCommand = new RelayCommand(DeleteCheckpointAction);
            ViewPictureCommand = new RelayCommand<Picture>(ViewPictureAction);
            SelectNotesCommand = new RelayCommand(SelectNotesAction);
            CancelSelectingNotesCommand = new RelayCommand(CancelSelectingNotesAction);
            DeleteNotesCommand = new RelayCommand<IEnumerable<Note>>(DeleteNotesAction);
            DeleteThumbnailCommand = new RelayCommand<Thumbnail>(DeleteThumbnailAction);
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
            set
            {
                if (_thumbnails == value) return;

                _thumbnails = value;
                RaisePropertyChanged("Thumbnails");
            }
        }

        private void InitializeThumbnails()
        {
            if (_thumbnails == null) _thumbnails = new ObservableCollection<Thumbnail>();
            if (_thumbnails.Count > 0) return;

            foreach (var picture in Checkpoint.Pictures)
            {
                _thumbnails.Add(new Thumbnail
                {
                    Picture = picture,
                    Placeholder = PICTURE_THUMBNAIL_PLACEHOLDER
                });
            }
        }

        public bool ShouldShowCheckpointMap
        {
            get { return _shouldShowCheckpointMap; }
            set
            {
                // not checking if value has been updated
                // this is to ensure the map can be hidden by default
                //
                var oldValue = _shouldShowCheckpointMap;
                _shouldShowCheckpointMap = value;

                // broadcast property changed message
                // the message will be caught in the code behind in order to update the UI
                //
                RaisePropertyChanged("ShouldShowCheckpointMap", oldValue, value, true);
            }
        }

        public bool IsNotesSelectionEnabled
        {
            get { return _isNotesSelectionEnabled; }
            set
            {
                if (_isNotesSelectionEnabled == value) return;

                _isNotesSelectionEnabled = value;

                try
                {
                    // for some reason an ArgumentOutOfRange exception is thrown here occasionally
                    // the exception appears to originate in one of the system libraries
                    //
                    RaisePropertyChanged("IsNotesSelectionEnabled");
                }
                catch (Exception) { /* nothing */ }
            }
        }

        public ICommand EditCheckpointCommand { get; private set; }

        public ICommand DeleteCheckpointCommand { get; private set; }

        public ICommand ViewPictureCommand { get; private set; }

        public ICommand SelectNotesCommand { get; private set; }

        public ICommand CancelSelectingNotesCommand { get; private set; }

        public ICommand DeleteNotesCommand { get; private set; }

        public ICommand DeleteThumbnailCommand { get; private set; }

        private void EditCheckpointAction()
        {
            Navigator.Navigate(String.Format("/Checkpoints/{0}/Edit", Checkpoint.ID));
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
            try
            {
                PictureStore.DeletePictures(Checkpoint.Pictures);
                _checkpointRepository.DeleteCheckpoint(Checkpoint);

                Navigator.GoBack();
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.CheckpointDeleteError, Resources.MessageBox_Error,
                    MessageBoxButton.OK);
            }
        }

        private void ViewPictureAction(Picture picture)
        {
            if (Checkpoint == null || picture == null) return;

            Navigator.Navigate(String.Format("/Pictures/{0}/Details", picture.ID));
        }

        private void SelectNotesAction()
        {
            if (Checkpoint == null || Checkpoint.Notes.Count == 0) return;

            IsNotesSelectionEnabled = true;
        }

        private void CancelSelectingNotesAction()
        {
            IsNotesSelectionEnabled = false;
        }

        private void DeleteNotesAction(IEnumerable<Note> notesToDelete)
        {
            if (notesToDelete == null) return;

            RepositoryFactory.NoteRepository.DeleteNotes(notesToDelete);
            
            // ensure the UI renders the most up to date checkpoint
            Checkpoint = RepositoryFactory.CheckpointRepository.FindCheckpoint(Checkpoint.ID);
        }

        private void DeleteThumbnailAction(Thumbnail thumbnail)
        {
            if (thumbnail == null) return;

            var userDecision = MessageBox.Show(Resources.ConfirmDeletePicture, Resources.Deleting,
                MessageBoxButton.OKCancel);

            if (userDecision != MessageBoxResult.OK) return;

            try
            {
                PictureDetailsViewModel.DeletePicture(thumbnail.Picture, RepositoryFactory.PictureRepository);
                Thumbnails.Remove(thumbnail);
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.PictureDeleteError, Resources.MessageBox_Error,
                    MessageBoxButton.OK);
            }
        }

        public void ResetViewModel()
        {
            _thumbnails = null;
            _checkpoint = null;
            ShouldShowCheckpointMap = false;
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            InitializeCheckpoint(TripPointConvert.ToInt32(GetParameter(e.View, "checkpointID")));
            DetermineCheckpointMapAvailability();
        }

        private void InitializeCheckpoint(int checkpointID)
        {
            if (checkpointID == -1) return;
            if (Checkpoint != null && Checkpoint.ID == checkpointID) return;

            RefreshCheckpoint(checkpointID);
        }

        private void RefreshCheckpoint(int checkpointID)
        {
            _checkpointRepository = RepositoryFactory.CheckpointRepository;

            Checkpoint = _checkpointRepository.FindCheckpoint(checkpointID) ?? new Checkpoint();
        }

        private void DetermineCheckpointMapAvailability()
        {
            // display the map provided that checkpoint location is available
            ShouldShowCheckpointMap = Checkpoint.Location != null && !Checkpoint.Location.IsUnknown;
        }

        public override void OnBackNavigatedTo()
        {
            base.OnBackNavigatedTo();

            RefreshCheckpoint();
            DeleteStaleThumbnailsIfNecessary();
        }

        private void RefreshCheckpoint()
        {
            if (Checkpoint == null) return;

            RefreshCheckpoint(Checkpoint.ID);
        }

        /// <summary>
        /// Scans through the thumbnails collection and deletes the items that are not in the
        /// checkpoint pictures collection.
        /// </summary>
        private void DeleteStaleThumbnailsIfNecessary()
        {
            if (_thumbnails == null || Checkpoint == null) return;
            if (Checkpoint.Pictures.Count == _thumbnails.Count) return;

            var staleThumbnails = (from thumbnail in _thumbnails
                                  where !Checkpoint.Pictures.Contains(thumbnail.Picture, new PictureComparer())
                                  select thumbnail).ToList();

            foreach (var thumbnail in staleThumbnails)
            {
                _thumbnails.Remove(thumbnail);
            }
        }

        #region PictureComparer
        
        class PictureComparer : IEqualityComparer<Picture>
        {
            public bool Equals(Picture left, Picture right)
            {
                if (Object.ReferenceEquals(left, right)) return true;

                if (Object.ReferenceEquals(left, null) || Object.ReferenceEquals(right, null)) return false;

                return left.ID == right.ID;
            }

            public int GetHashCode(Picture picture)
            {
                if (Object.ReferenceEquals(picture, null)) return 0;

                int titleHashCode = picture.Title == null ? 0 : picture.Title.GetHashCode();
                int idHashCode = picture.ID.GetHashCode();

                return titleHashCode ^ idHashCode;
            }
        }

        #endregion
    }
}

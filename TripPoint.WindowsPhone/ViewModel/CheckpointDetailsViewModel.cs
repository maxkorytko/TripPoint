using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.State.Data;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.I18N;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointDetailsViewModel : Base.TripPointViewModelBase
    {
        private Checkpoint _checkpoint;
        private ICollection<PictureThumbnail> _thumbnails;
        private bool _shouldShowCheckpointMap;
        private bool _isNotesSelectionEnabled;
        private ICheckpointRepository _checkpointRepository;
        
        public CheckpointDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            ShouldShowCheckpointMap = false;

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

        public ICollection<PictureThumbnail> Thumbnails
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
            if (_thumbnails == null) _thumbnails = new ObservableCollection<PictureThumbnail>();
            if (_thumbnails.Count > 0) return;

            foreach (var picture in Checkpoint.Pictures)
            {
                var thumbnail = new PictureThumbnail(new Uri("/Assets/Images/Dark/checkpoint.thumb.png", UriKind.Relative),
                    picture);

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

                var oldValue = _isNotesSelectionEnabled;
                _isNotesSelectionEnabled = value;
                
                RaisePropertyChanged("IsNotesSelectionEnabled", oldValue, value, true);
            }
        }

        public ICommand EditCheckpointCommand { get; private set; }

        public ICommand DeleteCheckpointCommand { get; private set; }

        public ICommand ViewPictureCommand { get; private set; }

        public ICommand SelectNotesCommand { get; private set; }

        public ICommand CancelSelectingNotesCommand { get; private set; }

        public ICommand DeleteNotesCommand { get; private set; }

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
            // TODO: ensure the app doesn't crash if exception occurs
            PictureStateManager.Instance.DeletePictures(Checkpoint.Pictures);
            _checkpointRepository.DeleteCheckpoint(Checkpoint);
            
            Navigator.GoBack();
        }

        private void ViewPictureAction(Picture picture)
        {
            if (Checkpoint == null || picture == null) return;

            Navigator.Navigate(String.Format("/Pictures/{0}/Details", picture.ID));
        }

        private void SelectNotesAction()
        {
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
            Checkpoint = _checkpointRepository.FindCheckpoint(Checkpoint.ID);
        }

        public void ResetViewModel()
        {
            Checkpoint = null;
            ShouldShowCheckpointMap = false;
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _checkpointRepository = RepositoryFactory.CheckpointRepository;
            
            InitializeCheckpoint(GetCheckpointID(e.View));
            DetermineCheckpointMapAvailability();

            // ensure thumbnails for deleted pictures are not displayed
            DeleteStaleThumbnailsIfNecessary();
        }

        private static int GetCheckpointID(PhoneApplicationPage view)
        {
            if (view == null) return -1;

            var parameter = view.TryGetQueryStringParameter("checkpointID");

            return TripPointConvert.ToInt32(parameter);
        }

        private void InitializeCheckpoint(int checkpointID)
        {
            if (Checkpoint != null) return;

            Checkpoint = _checkpointRepository.FindCheckpoint(checkpointID) ?? new Checkpoint();
        }

        private void DetermineCheckpointMapAvailability()
        {
            // display the map provided that checkpoint location is available
            ShouldShowCheckpointMap = Checkpoint.Location != null && !Checkpoint.Location.IsUnknown;
        }

        private void DeleteStaleThumbnailsIfNecessary()
        {
            if (_thumbnails == null) return;

            //var checkpoint = _checkpointRepository.FindCheckpoint(Checkpoint.ID);

            //if (checkpoint.Pictures.Count == Checkpoint.Pictures.Count) return;
            if (Checkpoint.Pictures.Count == _thumbnails.Count) return;

            var staleThumbnails = (from thumbnail in _thumbnails
                                  where !Checkpoint.Pictures.Contains(thumbnail.Picture, new PictureComparer())
                                  select thumbnail).ToList();

            foreach (var thumbnail in staleThumbnails)
            {
                _thumbnails.Remove(thumbnail);
            }

            //Checkpoint = checkpoint;
        }

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
    }
}

using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Microsoft.Phone.Tasks;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.I18N;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.State.Data;
using TripPoint.WindowsPhone.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CurrentTripViewModel : Base.TripCheckpointsViewModelBase
    {
        private Checkpoint _latestCheckpoint;
        private CameraCaptureTask _cameraCaptureTask;
        
        public CurrentTripViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            FinishTripCommand = new RelayCommand(FinishTripAction);
            PastTripsCommand = new RelayCommand(PastTripsAction);
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
            AddNotesCommand = new RelayCommand(AddNotesAction);
            AddPicturesCommand = new RelayCommand(AddPicturesAction);
            SettingsCommand = new RelayCommand(SettingsAction);
        }

        public Checkpoint LatestCheckpoint
        {
            get { return _latestCheckpoint; }
            private set
            {
                if (_latestCheckpoint == value) return;

                _latestCheckpoint = value;
                RaisePropertyChanged("LatestCheckpoint");
            }
        }

        public ICommand FinishTripCommand { get; private set; }

        public ICommand PastTripsCommand { get; private set; }

        public ICommand CreateCheckpointCommand { get; private set; }

        public ICommand AddNotesCommand { get; private set; }

        public ICommand AddPicturesCommand { get; private set; }

        public ICommand SettingsCommand { get; private set; }

        private void FinishTripAction()
        {
            var userDecision = MessageBox.Show(Resources.ConfirmFinishTrip, Resources.Confirm,
                MessageBoxButton.OKCancel);

            if (userDecision == MessageBoxResult.OK) FinishCurrentTrip();
        }

        private void FinishCurrentTrip()
        {
            Trip.EndDate = DateTime.Now;
            TripRepository.UpdateTrip(Trip);
            ResetViewModel();

            Navigator.NavigateWithoutHistory("/Trips");
        }

        public override void ResetViewModel()
        {
            base.ResetViewModel();

            LatestCheckpoint = null;
        }

        private void PastTripsAction()
        {
            Navigator.Navigate("/Trips");
        }

        private void CreateCheckpointAction()
        {
            Navigator.Navigate(string.Format("/Trip/{0}/Checkpoints/Create", Trip.ID));
        }

        private void AddNotesAction()
        {
            RememberLatestCheckpoint();
            Navigator.Navigate(string.Format("/Checkpoints/{0}/Add/Notes", LatestCheckpoint.ID));
        }

        private void AddPicturesAction()
        {
            InitializeCameraCaptureTask();
            _cameraCaptureTask.Show();
        }

        private void InitializeCameraCaptureTask()
        {
            if (_cameraCaptureTask != null) return;

            _cameraCaptureTask = new CameraCaptureTask();

            _cameraCaptureTask.Completed += (sender, e) =>
            {
                if (e.TaskResult != TaskResult.OK || e.ChosenPhoto == null) return;

                // the orientation of the photo taken by the phone camera
                // will be different from the phone orientation
                // ensure we always rotate the photo to portrait orientation
                //
                var photo = ImageUtils.RotateImageToPortrait(e.ChosenPhoto);

                SaveCapturedPhoto(new CapturedPicture(Path.GetFileNameWithoutExtension(e.OriginalFileName),
                    photo));
            };
        }

        private static void SaveCapturedPhoto(CapturedPicture picture)
        {
            try
            {
                StateManager.Instance.Set<CapturedPicture>(CheckpointAddPicturesViewModel.CAPTURED_PICTURE,
                    picture);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to save captured photo - {0}", new String[]{ ex.Message });
            }
        }

        private void SettingsAction()
        {
            Navigator.Navigate("/Application/Settings");
        }

        protected override Trip GetTrip(int tripID)
        {
            return TripRepository.CurrentTrip;
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {   
            base.OnNavigatedTo(e);

            SetLatestCheckpoint();
        }

        private void SetLatestCheckpoint()
        {
            if (Trip == null) return;

            LatestCheckpoint = Trip.Checkpoints.FirstOrDefault();
        }

        public override void OnBackNavigatedTo()
        {
            if (ShouldDisplayCameraCapturedPicture())
            {
                RememberLatestCheckpoint();
                Navigator.Navigate(String.Format("/Checkpoints/{0}/Add/Pictures", LatestCheckpoint.ID));
                return;
            }

            base.OnBackNavigatedTo();

            SetLatestCheckpoint();
        }

        private static bool ShouldDisplayCameraCapturedPicture()
        {
            return StateManager.Instance.Contains(CheckpointAddPicturesViewModel.CAPTURED_PICTURE);
        }

        private void RememberLatestCheckpoint()
        {
            RememberCheckpoint(LatestCheckpoint);
        }
    }
}

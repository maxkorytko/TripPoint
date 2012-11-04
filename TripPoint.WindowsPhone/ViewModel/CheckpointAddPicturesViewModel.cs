using System;
using System.Windows.Input;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.State;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointAddPicturesViewModel : TripPointViewModelBase
    {
        public static readonly string CAPTURED_PICTURE = "CheckpointAddPictureViewModel.CapturedPicture";

        private int _checkpointID;
        
        private CapturedPicture _picture;
        private string _pictureTitle;

        private ICheckpointRepository _checkpointRepository;
        private IPictureRepository _pictureRepository;

        public CheckpointAddPicturesViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddPictureCommand = new RelayCommand(AddPictureAction);
            CancelAddPictureCommand = new RelayCommand(CancelAddPictureAction);
        }

        public CapturedPicture Picture
        {
            get { return _picture; }
            set
            {
                if (_picture == value) return;

                _picture = value;
                RaisePropertyChanged("Picture");
            }
        }

        public string PictureTitle
        {
            get { return _pictureTitle; }
            set
            {
                if (_pictureTitle == value) return;

                _pictureTitle = value;
                RaisePropertyChanged("PictureTitle");
            }
        }

        public ICommand AddPictureCommand { get; private set; }

        public ICommand CancelAddPictureCommand { get; private set; }

        private void AddPictureAction()
        {
            var checkpoint = _checkpointRepository.FindCheckpoint(_checkpointID);

            var picture = CreatePicture(checkpoint);
            SavePicture(picture);

            Navigator.GoBack();
        }

        private Picture CreatePicture(Checkpoint checkpoint)
        {
            if (checkpoint == null) return null;

            var picture = new Picture
            {
                Checkpoint = checkpoint,
                FileName = Picture.FileName,
                Title = PictureTitle
            };

            return picture;
        }

        private void SavePicture(Picture picture)
        {
            if (picture == null) return;

            //_pictureRepository.SavePicture(picture);
        }

        private void CancelAddPictureAction()
        {
            Navigator.GoBack();
        }

        public override void OnNavigatedTo(Navigation.TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ResetViewModel();
            InitializePicture();

            _checkpointRepository = RepositoryFactory.CheckpointRepository;
            _checkpointID = GetCheckpointID(e.View);
        }

        private void InitializePicture()
        {
            Picture = StateManager.Instance.Get<CapturedPicture>(CAPTURED_PICTURE);
            StateManager.Instance.Remove(CAPTURED_PICTURE);
        }

        private void ResetViewModel()
        {
            Picture = null;
            PictureTitle = String.Empty;
        }

        private static int GetCheckpointID(PhoneApplicationPage view)
        {
            if (view == null) return -1;

            var checkpointIdParameter = view.TryGetQueryStringParameter("checkpointID");

            return TripPointConvert.ToInt32(checkpointIdParameter);
        } 
    }
}

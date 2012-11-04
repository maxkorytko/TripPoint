using System;
using System.Windows.Input;

using TripPoint.Model.Utils;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.State;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointAddPicturesViewModel : TripPointViewModelBase
    {
        public static readonly string CAPTURED_PICTURE = "CheckpointAddPictureViewModel.CapturedPicture";

        private CapturedPicture _picture;
        private string _pictureTitle;

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
            Logger.Log("add picture");
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
    }
}

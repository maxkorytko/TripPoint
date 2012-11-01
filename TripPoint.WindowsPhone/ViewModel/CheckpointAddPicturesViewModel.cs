using System;

using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.State;

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

        public override void OnNavigatedTo(Navigation.TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            InitializePicture();
        }

        private void InitializePicture()
        {
            Picture = StateManager.Instance.Get<CapturedPicture>(CAPTURED_PICTURE);
            StateManager.Instance.Remove(CAPTURED_PICTURE);
        }
    }
}

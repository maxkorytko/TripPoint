using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointAddPicturesViewModel : TripPointViewModelBase
    {
        public static readonly string CAPTURED_PICTURE = "CheckpointAddPictureViewModel.CapturedPicture";

        private int _checkpointID;

        private Picture _picture;

        private ICheckpointRepository _checkpointRepository;

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

        public Picture Picture
        {
            get { return _picture; }
            set
            {
                if (_picture == value) return;

                _picture = value;
                RaisePropertyChanged("Picture");
            }
        }

        public ICommand AddPictureCommand { get; private set; }

        public ICommand CancelAddPictureCommand { get; private set; }

        private void AddPictureAction()
        {
            var checkpoint = _checkpointRepository.FindCheckpoint(_checkpointID);
            
            if (checkpoint != null)
            {
                checkpoint.Pictures.Add(Picture);
                _checkpointRepository.UpdateCheckpoint(checkpoint);

                // TODO:
                // save picture to isolated storage.
                // consider applying a strategy pattern for saving/restoring objects from IsoStorage
            }

            Navigator.GoBack();
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
            var capturedPicture = StateManager.Instance.Get<CapturedPicture>(CAPTURED_PICTURE);
            StateManager.Instance.Remove(CAPTURED_PICTURE);

            Picture = CreatePicture(capturedPicture);
        }

        private static Picture CreatePicture(CapturedPicture source)
        {
            var picture = new DisplayablePicture();

            if (source != null)
            {
                picture.FileName = source.FileName;
                picture.RawBytes = source.RawBytes;
                picture.Title = String.Empty;
            }

            return picture;
        }

        private void ResetViewModel()
        {
            Picture = null;
        }

        private static int GetCheckpointID(PhoneApplicationPage view)
        {
            if (view == null) return -1;

            var checkpointIdParameter = view.TryGetQueryStringParameter("checkpointID");

            return TripPointConvert.ToInt32(checkpointIdParameter);
        }

        public class DisplayablePicture : Picture
        {
            private ImageSource _source;

            public ImageSource Source
            {
                get
                {
                    if (_source == null)
                    {
                        using (var stream = new MemoryStream(RawBytes))
                        {
                            _source = ImageUtils.CreateWriteableBitmapFromStream(stream);
                        }
                    }

                    return _source;
                }
            }
        }
    }
}

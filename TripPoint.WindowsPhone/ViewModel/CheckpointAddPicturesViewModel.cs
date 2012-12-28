using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Threading;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.State.Data;
using TripPoint.WindowsPhone.Utils;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointAddPicturesViewModel : Base.TripPointViewModelBase
    {
        public static readonly string CAPTURED_PICTURE = "CheckpointAddPictureViewModel.CapturedPicture";

        private int _checkpointID;
        private Picture _picture;
        private bool _isSavingPicture;
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

        public bool IsSavingPicture
        {
            get { return _isSavingPicture; }
            private set
            {
                if (_isSavingPicture == value) return;

                _isSavingPicture = value;
                RaisePropertyChanged("IsSavingPicture");
            }
        }

        public ICommand AddPictureCommand { get; private set; }

        public ICommand CancelAddPictureCommand { get; private set; }

        private void AddPictureAction()
        {
            IsSavingPicture = true;

            var worker = new BackgroundWorker();

            worker.DoWork += (sender, args) => { SavePicture(); };
            worker.RunWorkerCompleted += (sender, args) => { Navigator.GoBack(); };
            worker.RunWorkerAsync();
        }

        private void SavePicture()
        {
            var checkpoint = _checkpointRepository.FindCheckpoint(_checkpointID);

            if (checkpoint == null) return;

            checkpoint.Pictures.Add(Picture);
            _checkpointRepository.UpdateCheckpoint(checkpoint);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // scale the picture in order to save disk space
                // it will also help speed up loading the picture
                //
                ScalePicture(0.75f);

                lock (Picture)
                {
                    Monitor.Pulse(Picture);
                }
            });

            // wait until scaling is complete before saving the image
            lock (Picture)
            {
                Monitor.Wait(Picture);
            }

            PictureStateManager.Instance.SavePicture(Picture);
        }

        private void CancelAddPictureAction()
        {
            Navigator.GoBack();
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ResetViewModel();
            InitializePicture();

            _checkpointID = GetCheckpointID(e.View);
            _checkpointRepository = RepositoryFactory.CheckpointRepository;
        }

        private void ResetViewModel()
        {
            Picture = null;
            IsSavingPicture = false;
        }

        private void InitializePicture()
        {
            var capturedPicture = StateManager.Instance.Get<CapturedPicture>(CAPTURED_PICTURE);
            StateManager.Instance.Remove(CAPTURED_PICTURE);

            Picture = CreatePicture(capturedPicture);
        }

        private static Picture CreatePicture(CapturedPicture source)
        {
            var picture = new Picture();
            
            picture.DateTaken = DateTime.Now;

            if (source != null)
            {
                picture.FileName = source.FileName;
                picture.RawBytes = source.RawBytes;
                picture.Title = String.Empty;
            }

            return picture;
        }

        private void ScalePicture(float scalingFactor)
        {
            if (Picture.RawBytes == null) return;

            var bitmap = ImageUtils.CreateWriteableBitmapFromBytes(Picture.RawBytes);

            if (bitmap == null) return;

            bitmap = bitmap.Resize(Convert.ToInt32(bitmap.PixelWidth * scalingFactor), 
                Convert.ToInt32(bitmap.PixelHeight * scalingFactor),
                WriteableBitmapExtensions.Interpolation.Bilinear);

            Picture.RawBytes = TripPointConvert.ToBytes(bitmap.SaveJpeg());
        }

        private static int GetCheckpointID(PhoneApplicationPage view)
        {
            if (view == null) return -1;

            var checkpointIdParameter = view.TryGetQueryStringParameter("checkpointID");

            return TripPointConvert.ToInt32(checkpointIdParameter);
        }
    }
}

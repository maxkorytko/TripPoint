using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Threading;
using System.Windows.Media.Imaging;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.State.Data;
using TripPoint.WindowsPhone.Utils;
using TripPoint.I18N;
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

        public CapturedPicture CapturedPicture { get; set; }

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

            worker.DoWork += (sender, args) => { AddPicture(); };
            worker.RunWorkerCompleted += (sender, args) => { EnsurePictureWasAdded(args.Error); };
            worker.RunWorkerAsync();
        }

        private void AddPicture()
        {
            if (Picture == null) return;

            ScalePicture();
            SavePicture();
        }

        /// <summary>
        /// Scales the picture
        /// This method will always run on the UI thread
        /// The calling thread will block until scaling is finished
        /// </summary>
        private void ScalePicture()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
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
        }

        private void ScalePicture(float scalingFactor)
        {
            if (CapturedPicture.RawBytes == null) return;

            var bitmap = ImageUtils.CreateWriteableBitmapFromBytes(CapturedPicture.RawBytes);

            if (bitmap == null) return;

            bitmap = bitmap.Resize(
                Convert.ToInt32(bitmap.PixelWidth * scalingFactor),
                Convert.ToInt32(bitmap.PixelHeight * scalingFactor),
                WriteableBitmapExtensions.Interpolation.Bilinear);

            CapturedPicture.RawBytes = TripPointConvert.ToBytes(bitmap.SaveJpeg());
        }

        /// <summary>
        /// Saves the picture permanently
        /// </summary>
        private void SavePicture()
        {
            var checkpoint = _checkpointRepository.FindCheckpoint(_checkpointID);

            if (checkpoint == null) return;

            checkpoint.Pictures.Add(Picture);
            _checkpointRepository.UpdateCheckpoint(checkpoint);

            PictureStore.SavePicture(Picture, CapturedPicture.RawBytes);
        }

        private void EnsurePictureWasAdded(Exception error)
        {
            if (error != null)
            {
                MessageBox.Show(Resources.PictureAddError, Resources.MessageBox_Error, MessageBoxButton.OK);
            }

            Navigator.GoBack();
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

            _checkpointID = TripPointConvert.ToInt32(GetParameter(e.View, "checkpointID"));
            _checkpointRepository = RepositoryFactory.CheckpointRepository;
        }

        public void ResetViewModel()
        {
            Picture = null;
            IsSavingPicture = false;
        }

        private void InitializePicture()
        {
            CapturedPicture = StateManager.Instance.Get<CapturedPicture>(CAPTURED_PICTURE);
            StateManager.Instance.Remove(CAPTURED_PICTURE);

            Picture = CreatePicture(CapturedPicture);
        }

        private static Picture CreatePicture(CapturedPicture source)
        {
            var picture = new Picture();
            
            picture.DateTaken = DateTime.Now;

            if (source != null)
            {
                picture.FileName = source.FileName;
                picture.Title = String.Empty;
            }

            return picture;
        }
    }
}

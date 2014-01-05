using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Media;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.I18N;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.Navigation;
using GalaSoft.MvvmLight.Command;


namespace TripPoint.WindowsPhone.ViewModel
{
    public class PictureDetailsViewModel : Base.TripPointViewModelBase
    {
        private TripPoint.Model.Domain.Picture _picture;
        private Collection<TripPoint.Model.Domain.Picture> _pictures;
        private bool _savingPictureToMediaLibrary;

        public PictureDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            _pictures = new ObservableCollection<TripPoint.Model.Domain.Picture>();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SavePictureCommand = new RelayCommand(SavePictureAction);
            DeletePictureCommand = new RelayCommand(DeletePictureAction);
        }

        public TripPoint.Model.Domain.Picture Picture
        {
            get { return _picture; }
            set
            {
                if (_picture == value) return;

                _picture = value;
                RaisePropertyChanged("Picture");
            }
        }

        public Collection<TripPoint.Model.Domain.Picture> Pictures
        {
            get { return _pictures; }
            set
            {
                if (_pictures == value) return;

                _pictures = value;
                RaisePropertyChanged("Pictures");
            }
        }

        public bool SavingPictureToMediaLibrary
        {
            get { return _savingPictureToMediaLibrary; }
            set
            {
                if (_savingPictureToMediaLibrary == value) return;

                _savingPictureToMediaLibrary = value;
                RaisePropertyChanged("SavingPictureToMediaLibrary");
            }
        }

        public ICommand SavePictureCommand { get; private set; }

        public ICommand DeletePictureCommand { get; private set; }

        private void SavePictureAction()
        {
            var userDecision = MessageBox.Show(Resources.ConfirmSavingPictureToPictureHub,
                Resources.SavingPicture, MessageBoxButton.OKCancel);

            if (userDecision == MessageBoxResult.OK)
            {
                SavingPictureToMediaLibrary = true;
                SavePictureToMediaLibrary();
                SavingPictureToMediaLibrary = false;
            }   
        }

        private void SavePictureToMediaLibrary()
        {
            try
            {
                using (var mediaLibrary = new MediaLibrary())
                {
                    var name = Picture.Title;
                    if (String.IsNullOrWhiteSpace(name)) name = Picture.FileName;

                    mediaLibrary.SavePicture(name, Picture.RawBytes);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.SavePictureToPictureHubFailed,
                    Resources.MessageBox_Error, MessageBoxButton.OK);
            }
        }

        private void DeletePictureAction()
        {
            var userDecision = MessageBox.Show(Resources.ConfirmDeletePicture, Resources.Deleting,
                MessageBoxButton.OKCancel);

            if (userDecision != MessageBoxResult.OK) return;

            var adjacentPicture = GetAdjacentPicture();

            DeletePicture(Picture);

            if (Pictures.Count == 0) Navigator.GoBack();
            else Picture = adjacentPicture;
        }

        /// <summary>
        /// Returns a picture that's to the right of the current picture if there is one.
        /// Otherwise, returns a picture that's to the left of the current pictures.
        /// Returns the current picture if it's the single item in the pictures collection.
        /// </summary>
        private TripPoint.Model.Domain.Picture GetAdjacentPicture()
        {
            if (Pictures.Count <= 1) return Picture;

            var index = Pictures.IndexOf(Picture);
            
            return (index == Pictures.Count - 1) ? Pictures[--index] : Pictures[++index];
        }

        /// <summary>
        /// Deletes the given picture from the repository
        /// </summary>
        /// <param name="pictureToDelete"></param>
        private void DeletePicture(TripPoint.Model.Domain.Picture pictureToDelete)
        {
            try
            {
                PictureStateManager.Instance.DeletePicture(pictureToDelete);
                RepositoryFactory.PictureRepository.DeletePicture(pictureToDelete);
                Pictures.Remove(pictureToDelete);
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.PictureDeleteError, Resources.MessageBox_Error,
                    MessageBoxButton.OK);
            }
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            InitializePicture(TripPointConvert.ToInt32(GetParameter(e.View, "pictureID")));
            InitializePicturesCollection();
        }

        private void InitializePicture(int pictureID)
        {
            var picture = RepositoryFactory.PictureRepository.FindPicture(pictureID);

            if (picture != null && picture != Picture) Picture = picture;
        }

        private void InitializePicturesCollection()
        {
            if (Picture == null) return;

            LoadPicturesFromCheckpoint(Picture.Checkpoint);
        }

        private void LoadPicturesFromCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint == null) return;
            if (checkpoint.Pictures.Count == 0) return;

            Pictures.Clear();

            foreach (var picture in checkpoint.Pictures)
            {
                picture.RawBytes = PictureStateManager.Instance.LoadPicture(picture);
                Pictures.Add(picture);
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Media;

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.I18N;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.Navigation;
using TripPoint.Model.Data.Repository;


namespace TripPoint.WindowsPhone.ViewModel
{
    public class PictureDetailsViewModel : Base.TripPointViewModelBase
    {
        private Checkpoint _checkpoint;
        private TripPoint.Model.Domain.Picture _picture;
        private bool _savingPictureToMediaLibrary;

        public PictureDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SavePictureCommand = new RelayCommand(SavePictureAction);
            DeletePictureCommand = new RelayCommand(DeletePictureAction);
        }

        public Checkpoint Checkpoint
        {
            get { return _checkpoint; }
            set
            {
                if (_checkpoint == value) return;

                _checkpoint = value;
                RaisePropertyChanged("Checkpoint");
            }
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

                    mediaLibrary.SavePicture(name, PictureStore.LoadPicture(Picture));
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

            if (Checkpoint.Pictures.Count == 0) Navigator.GoBack();
            else Picture = adjacentPicture;
        }

        /// <summary>
        /// Returns a picture that's to the right of the current picture if there is one.
        /// Otherwise, returns a picture that's to the left of the current pictures.
        /// Returns the current picture if it's the single item in the pictures collection.
        /// </summary>
        private TripPoint.Model.Domain.Picture GetAdjacentPicture()
        {
            if (Checkpoint.Pictures.Count <= 1) return Picture;

            var index = Checkpoint.Pictures.IndexOf(Picture);

            return (index == Checkpoint.Pictures.Count - 1) ? Checkpoint.Pictures[--index]
                                                            : Checkpoint.Pictures[++index];
        }

        private void DeletePicture(TripPoint.Model.Domain.Picture pictureToDelete)
        {
            try
            {
                DeletePicture(pictureToDelete, RepositoryFactory.PictureRepository);
                Checkpoint.Pictures.Remove(pictureToDelete);
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.PictureDeleteError, Resources.MessageBox_Error,
                    MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Deletes the given picture from the repository
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="repository"></param>
        public static void DeletePicture(TripPoint.Model.Domain.Picture picture,
            IPictureRepository repository)
        {
            if (picture == null) throw new ArgumentException("picture");
            if (repository == null) throw new ArgumentException("repository");

            PictureStore.DeletePicture(picture);
            repository.DeletePicture(picture);
        }

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            InitializePicture(TripPointConvert.ToInt32(GetParameter(e.View, "pictureID")));
            InitializeCheckpoint();
        }

        private void InitializePicture(int pictureID)
        {
            if (Picture != null) return;

            Picture = RepositoryFactory.PictureRepository.FindPicture(pictureID);
        }

        private void InitializeCheckpoint()
        {
            if (Picture == null) return;
            if (Checkpoint != null) return;

            Checkpoint = Picture.Checkpoint;
        }

        public void ResetViewModel()
        {
            Picture = null;
            Checkpoint = null;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;
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
                SavePicture();
                SavingPictureToMediaLibrary = false;
            }   
        }

        private void SavePicture()
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

            if (userDecision == MessageBoxResult.OK)
                DeletePicture();
        }

        private void DeletePicture()
        {
            try
            {
                PictureStateManager.Instance.DeletePicture(Picture);
                RepositoryFactory.PictureRepository.DeletePicture(Picture);

                Navigator.GoBack();
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

            var pictureID = TripPointConvert.ToInt32(GetParameter(e.View, "pictureID"));
            Picture = RepositoryFactory.PictureRepository.FindPicture(pictureID);
            
            LoadPicture();
        }

        // TO-DO: move this method to TripPointViewModelBase
        private static string GetParameter(PhoneApplicationPage view, string parameterName)
        {
            if (view == null) return String.Empty;

            return view.TryGetQueryStringParameter(parameterName);
        }

        private void LoadPicture()
        {
            if (Picture == null) return;

            Picture.RawBytes = PictureStateManager.Instance.LoadPicture(Picture);
        }
    }
}

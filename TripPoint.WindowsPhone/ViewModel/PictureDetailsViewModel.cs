using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;

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
        private Picture _picture;

        public PictureDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            DeletePictureCommand = new RelayCommand(DeletePictureAction);
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

        public ICommand DeletePictureCommand { get; private set; }

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
            }
            catch (Exception ex)
            {
                Logger.Log(this, String.Format("Could not delete picture: {0}", ex.Message));
            }
            finally
            {
                Navigator.GoBack();
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

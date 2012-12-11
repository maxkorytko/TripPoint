using System;
using System.Linq;
using Microsoft.Phone.Controls;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class PictureDetailsViewModel : TripPointViewModelBase
    {
        private Picture _picture;

        public PictureDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            
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

        public override void OnNavigatedTo(TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var checkpointID = TripPointConvert.ToInt32(GetParameter(e.View, "checkpointID"));
            var pictureID = TripPointConvert.ToInt32(GetParameter(e.View, "pictureID"));
            
            InitializePicture(checkpointID, pictureID);
            LoadPicture();
        }

        private static string GetParameter(PhoneApplicationPage view, string parameterName)
        {
            if (view == null) return String.Empty;

            return view.TryGetQueryStringParameter(parameterName);
        }

        private void InitializePicture(int checkpointID, int pictureID)
        {
            var checkpoint = RepositoryFactory.CheckpointRepository.FindCheckpoint(checkpointID);

            if (checkpoint != null)
            {
                Picture = checkpoint.Pictures.SingleOrDefault<Picture>(picture => picture.ID == pictureID);
            }
        }

        private void LoadPicture()
        {
            if (Picture == null) return;

            Picture.RawBytes = RepositoryFactory.PictureRepository.LoadPictureAsBytes(Picture);
        }
    }
}

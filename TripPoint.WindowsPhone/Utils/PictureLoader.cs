using System.IO;
using System.Windows.Media.Imaging;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;

namespace TripPoint.WindowsPhone.Utils
{
    public class PictureLoader
    {
        private static PictureLoader _instance;
        
        private static IPictureRepository _pictureRepository;

        private PictureLoader()
        {
        }

        public static PictureLoader Instance
        {
            get
            {
                if (_instance == null) InitializeInstance();

                return _instance;
            }
        }

        private static void InitializeInstance()
        {
            _instance = new PictureLoader();
            _pictureRepository = RepositoryFactory.Create().PictureRepository;
        }

        public void LoadThumbnail(Thumbnail thumbnail)
        {
            if (thumbnail == null) return;

            var pictureBytes = thumbnail.Picture.RawBytes;

            if (pictureBytes == null || pictureBytes.Length == 0)
                pictureBytes = _pictureRepository.LoadPictureAsBytes(thumbnail.Picture);

            thumbnail.Source = ImageUtils.CreateBitmapFromBytes(pictureBytes);
        }
    }
}

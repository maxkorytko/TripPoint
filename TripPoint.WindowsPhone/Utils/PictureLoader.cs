using System.IO;
using System.Windows.Media.Imaging;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.WindowsPhone.State.Data;

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

        public byte[] LoadPicture(Picture picture)
        {
            if (picture == null) return new byte[0];

            return _pictureRepository.LoadPictureAsBytes(picture) ?? new byte[0];
        }
    }
}

using System.Linq;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public class DatabasePictureRepository : DatabaseRepository, IPictureRepository
    {
        public DatabasePictureRepository(TripPointDataContext dataContext)
            : base(dataContext)
        {
        }

        public Picture FindPicture(int pictureID)
        {
            return DataContext.Pictures.SingleOrDefault(p => p.ID == pictureID);
        }

        public void DeletePicture(Picture picture)
        {
            if (picture == null) return;

            var pictureEntity = DataContext.Pictures.SingleOrDefault(p => p.ID == picture.ID);

            if (pictureEntity == null) return;

            pictureEntity.Checkpoint = null;
            DataContext.Pictures.DeleteOnSubmit(pictureEntity);
            DataContext.SubmitChanges();
        }
    }
}

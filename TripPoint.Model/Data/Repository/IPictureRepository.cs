using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface IPictureRepository
    {
        /// <summary>
        /// Searches for a picture with the given ID
        /// </summary>
        /// <param name="pictureID">ID of the picture to look for</param>
        /// <returns></returns>
        Picture FindPicture(int pictureID);

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="picture"></param>
        void DeletePicture(Picture picture);
    }
}

﻿using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface IPictureRepository
    {
        /// <summary>
        /// Restores byte array containing the picture 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        byte[] LoadPictureAsBytes(Picture picture);
        
        /// <summary>
        /// Persists byte array containing the picture
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        bool SavePictureAsBytes(Picture picture);

        void DeletePictures(IEnumerable<Picture> picturesToDelete);
    }
}
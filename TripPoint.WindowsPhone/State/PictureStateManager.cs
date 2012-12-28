using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.WindowsPhone.State
{
    public class PictureStateManager
    {
        private static readonly byte[] EMPTY_BYTE_ARRAY = new byte[0];

        private static PictureStateManager _instance;

        private PictureStateManager()
        {
        }

        public static PictureStateManager Instance
        {
            get
            {
                if (_instance == null) _instance = new PictureStateManager();
                return _instance;
            }
        }

        /// <summary>
        /// Restores byte array containing the picture 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public byte[] LoadPicture(Picture picture)
        {
            if (picture == null) return EMPTY_BYTE_ARRAY;

            byte[] pictureBytes = null;

            try
            {
                using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var filePath = GetFilePath(picture);

                    using (var fileStream = isolatedStorage.OpenFile(filePath, FileMode.Open, FileAccess.Read))
                    {
                        pictureBytes = new byte[fileStream.Length];
                        fileStream.Read(pictureBytes, 0, pictureBytes.Length);
                    }
                }
            }
            catch (Exception)
            {
                pictureBytes = EMPTY_BYTE_ARRAY;
            }

            return pictureBytes;
        }

        private static string GetFilePath(Picture picture)
        {
            return Path.Combine(GetDirectoryPath(picture), picture.FileName);
        }

        private static string GetDirectoryPath(Picture picture)
        {
            var tripID = Convert.ToString(picture.Checkpoint.Trip.ID);

            return Path.Combine(Path.Combine("Trips", tripID), "Pictures");
        }

        /// <summary>
        /// Persists byte array containing the picture
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public void SavePicture(Picture picture)
        {
            if (picture == null) return;

            if (String.IsNullOrWhiteSpace(picture.FileName))
                throw new InvalidOperationException("Picture file name must not be blank");

            WritePictureToIsolatedStorage(picture);
        }

        private static void WritePictureToIsolatedStorage(Picture picture)
        {
            EnsurePathExists(GetDirectoryPath(picture));
            WritePictureToIsolatedStorage(picture, GetFilePath(picture));
        }

        private static void EnsurePathExists(string path)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.DirectoryExists(path)) return;

                isolatedStorage.CreateDirectory(path);
            }
        }

        private static void WritePictureToIsolatedStorage(Picture picture, string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath");

            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fileStream = new IsolatedStorageFileStream(filePath, FileMode.Create, isolatedStorage))
                {
                    using (var writer = new BinaryWriter(fileStream))
                    {
                        writer.Write(picture.RawBytes);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="pictureToDelete"></param>
        public void DeletePicture(Picture pictureToDelete)
        {
            if (pictureToDelete == null) return;

            var filePath = GetFilePath(pictureToDelete);

            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(filePath))
                    isolatedStorage.DeleteFile(filePath);

                // TODO: delete the directory if it's empty
            }
        }

        /// <summary>
        /// Deletes a number of pictures
        /// </summary>
        /// <param name="picturesToDelete"></param>
        public void DeletePictures(IEnumerable<Picture> picturesToDelete)
        {
            foreach (var picture in picturesToDelete)
            {
                DeletePicture(picture);
            }
        }
    }
}

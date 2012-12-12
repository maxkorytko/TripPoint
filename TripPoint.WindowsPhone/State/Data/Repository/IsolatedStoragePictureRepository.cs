using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository;

namespace TripPoint.WindowsPhone.State.Data.Repository
{
    /// <summary>
    /// Concrete implementation of the IPictureRepository interface
    /// Uses Isolated Storage for persisting and restoring pictures 
    /// </summary>
    public class IsolatedStoragePictureRepository : IPictureRepository
    {
        private static readonly byte[] EMPTY_BYTE_ARRAY = new byte[0];

        // Override
        public byte[] LoadPictureAsBytes(Picture picture)
        {
            if (picture == null) return EMPTY_BYTE_ARRAY;

            var filePath = GetFilePath(picture);

            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isolatedStorage.FileExists(filePath)) return EMPTY_BYTE_ARRAY;

                using (var fileStream = isolatedStorage.OpenFile(filePath, FileMode.Open, FileAccess.Read))
                {
                    var pictureBytes = new byte[fileStream.Length];

                    fileStream.Read(pictureBytes, 0, pictureBytes.Length);

                    return pictureBytes;
                }
            }
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

        // Override
        public bool SavePictureAsBytes(Picture picture)
        {
            if (picture == null) return false;

            if (String.IsNullOrWhiteSpace(picture.FileName))
                throw new InvalidOperationException("Picture file name must not be blank");

            try
            {
                WritePictureToIsolatedStorage(picture);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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

        public void DeletePictures(IEnumerable<Picture> picturesToDelete)
        {
            foreach (var picture in picturesToDelete)
            {
                DeletePicture(picture);
            }
        }

        private void DeletePicture(Picture pictureToDelete)
        {
            if (pictureToDelete == null) return;

            var filePath = GetFilePath(pictureToDelete);

            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(filePath))
                    isolatedStorage.DeleteFile(filePath);
            }
        }
    }
}
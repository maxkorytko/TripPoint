using System;
using System.IO;
using System.IO.IsolatedStorage;

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

        public byte[] LoadPictureAsBytes(Picture picture)
        {
            if (picture == null) return EMPTY_BYTE_ARRAY;

            var path = Path.Combine(GetDirectoryPath(picture), picture.FileName);

            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isolatedStorage.FileExists(path)) return EMPTY_BYTE_ARRAY;

                using (var fileStream = isolatedStorage.OpenFile(path, FileMode.Open, FileAccess.Read))
                {
                    var pictureBytes = new byte[fileStream.Length];

                    fileStream.Read(pictureBytes, 0, pictureBytes.Length);

                    return pictureBytes;
                }
            }
        }

        private static string GetDirectoryPath(Picture picture)
        {
            var tripID = Convert.ToString(picture.Checkpoint.Trip.ID);

            return Path.Combine(Path.Combine("Trips", tripID), "Pictures");
        }

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
            var path = GetDirectoryPath(picture);

            EnsurePathExists(path);
            WritePictureToIsolatedStorage(picture, path);
        }

        private static void EnsurePathExists(string path)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.DirectoryExists(path)) return;

                isolatedStorage.CreateDirectory(path);
            }
        }

        private static void WritePictureToIsolatedStorage(Picture picture, string path)
        {
            var fileName = Path.Combine(path, picture.FileName);

            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, isolatedStorage))
                {
                    using (var writer = new BinaryWriter(fileStream))
                    {
                        writer.Write(picture.RawBytes);
                    }
                }
            }
        }
    }
}
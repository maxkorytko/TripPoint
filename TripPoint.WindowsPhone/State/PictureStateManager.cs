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

            try
            {
                WritePictureToIsolatedStorage(picture);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        private static void WritePictureToIsolatedStorage(Picture picture)
        {
            EnsureDirectoryExists(GetDirectoryPath(picture));
            WritePictureToIsolatedStorage(picture, GetFilePath(picture));
        }

        private static void EnsureDirectoryExists(string path)
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
            try
            {
                using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    DeletePicture(pictureToDelete, isolatedStorage);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a picture from isolated storage
        /// </summary>
        /// <param name="pictureToDelete"></param>
        /// <param name="isolatedStorage"></param>
        private static void DeletePicture(Picture pictureToDelete, IsolatedStorageFile isolatedStorage)
        {
            if (pictureToDelete == null || isolatedStorage == null) return;

            var filePath = GetFilePath(pictureToDelete);

            if (isolatedStorage.FileExists(filePath))
            {
                isolatedStorage.DeleteFile(filePath);
                DeleteDirectoryIfEmpty(GetDirectoryPath(pictureToDelete), isolatedStorage);
            }
        }

        /// <summary>
        /// Recursively deletes empty directories by moving from child to parent
        /// Every time a directory is deletes, its parent directory is examined
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isolatedStorage"></param>
        private static void DeleteDirectoryIfEmpty(string path, IsolatedStorageFile isolatedStorage)
        {
            if (String.IsNullOrWhiteSpace(path)) return;
            if (isolatedStorage == null) return;

            // this only looks for files and does not count sub-directories
            var fileNames = isolatedStorage.GetFileNames(Path.Combine(path, "*"));

            if (fileNames.Length > 0) return;

            try
            {
                // an exception will be thrown unless the directory is empty
                isolatedStorage.DeleteDirectory(path);

                // delete parent directory
                DeleteDirectoryIfEmpty(GetParentDirectoryPath(path), isolatedStorage);
            }
            catch (Exception)
            {
                // swallow
            }
        }

        /// <summary>
        /// Constructs the new path which is the same as the original path minus the last directory name
        /// The new path does not contain the directory separator at the end
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetParentDirectoryPath(string path)
        {
            if (path == null) return null;
            if (String.IsNullOrWhiteSpace(path)) return String.Empty;

            int lastSeparatorIndex = path.LastIndexOf(Path.DirectorySeparatorChar);
            
            lastSeparatorIndex = lastSeparatorIndex != -1 ? lastSeparatorIndex
                : path.LastIndexOf(Path.AltDirectorySeparatorChar);

            if (lastSeparatorIndex == -1) return String.Empty;

            return path.Substring(0, lastSeparatorIndex);
        }

        /// <summary>
        /// Deletes a number of pictures
        /// </summary>
        /// <param name="picturesToDelete"></param>
        public void DeletePictures(IEnumerable<Picture> picturesToDelete)
        {
            try
            {
                using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    foreach (var picture in picturesToDelete)
                    {
                        DeletePicture(picture, isolatedStorage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}

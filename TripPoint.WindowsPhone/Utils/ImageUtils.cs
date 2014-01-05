using System;
using System.IO;
using System.Windows.Media.Imaging;

using TripPoint.WindowsPhone.Utils.ExifLib;

namespace TripPoint.WindowsPhone.Utils
{
    /// <summary>
    /// Represents a collection of utility methods for working with images
    /// </summary>
    public class ImageUtils
    {
        /// <summary>
        /// Rotates the photo stream to portrait orientation
        /// Use this method when you have a photo, which is not in portrait orientation,
        /// but you want to display it in portrait orientation
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Stream RotateImageToPortrait(Stream source)
        {
            try
            {
                var initialSourceStreamPosition = source.Position;

                source.Position = 0;
                
                ExifOrientation orientation = ExifReader.ReadJpeg(source).Orientation;
                
                var degreesToPortrait = GetDegreesToPortrait(orientation);
                var bitmap = CreateWriteableBitmapFromStream(source);
                var rotatedBitmap = bitmap.Rotate(degreesToPortrait);

                source.Position = initialSourceStreamPosition;

                return rotatedBitmap.SaveJpeg();
            }
            catch (Exception)
            {
                return new MemoryStream();
            }
        }

        /// <summary>
        /// Determines the number of degrees between current and portrait orientations
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        private static int GetDegreesToPortrait(ExifOrientation orientation)
        {
            switch (orientation)
            {
                case ExifOrientation.TopRight:
                    return 90;
                case ExifOrientation.BottomRight:
                    return 180;
                case ExifOrientation.BottomLeft:
                    return 270;
                case ExifOrientation.TopLeft:
                case ExifOrientation.Undefined:
                default:
                    return 0;
            }
        }

        private static WriteableBitmap CreateWriteableBitmapFromStream(Stream source)
        {
            if (source == null) return null;

            var bitmap = new BitmapImage();
            bitmap.SetSource(source);

            return new WriteableBitmap(bitmap);
        }

        /// <summary>
        /// Creates an instance of WriteableBitmap from the byte array
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static WriteableBitmap CreateWriteableBitmapFromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            using (var stream = new MemoryStream(bytes))
            {
                return CreateWriteableBitmapFromStream(stream);
            }
        }

        /// <summary>
        /// Creates an instance of BitmapImage from the byte array
        /// Decodes the image in the background by default
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static BitmapImage CreateBitmapFromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            using (var source = new MemoryStream(bytes))
            {
                var bitmap = new BitmapImage();
                bitmap.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                bitmap.SetSource(source);

                return bitmap;
            }
        }
    }
}

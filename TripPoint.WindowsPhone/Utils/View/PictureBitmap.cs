using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using TripPoint.Model.Domain;
using TripPoint.WindowsPhone.State;

namespace TripPoint.WindowsPhone.Utils.View
{
    /// <summary>
    /// Implements attached properties that simplify displaying Picture objects using XAML
    /// </summary>
    public class PictureBitmap : DependencyObject
    {
        #region Source Attached Property
        
        public static void SetSource(DependencyObject o, Picture value)
        {
            o.SetValue(SourceProperty, value);
        }

        public static Picture GetSource(DependencyObject o)
        {
            return o.GetValue(SourceProperty) as Picture;
        }

        /// <summary>
        /// Creates a BitmapSource object which represents the Picture object
        /// Sets Image.Source property to the created BitmapSource object
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached(
                "Source",
                typeof(Picture),
                typeof(PictureBitmap),
                new PropertyMetadata(OnSourceChanged));

        private static void OnSourceChanged(DependencyObject owner,
            DependencyPropertyChangedEventArgs args)
        {
            SetImageSource(owner as Image, args.NewValue as Picture);
        }

        private static void SetImageSource(Image image, Picture picture)
        {
            if (image == null) return;

            image.Source = GetImageSource(picture);
        }

        #endregion

        #region Thumbnail Attached Property

        public static void SetThumbnail(DependencyObject o, Thumbnail value)
        {
            o.SetValue(ThumbnailProperty, value);
        }

        public static Thumbnail GetThumbnail(DependencyObject o)
        {
            return o.GetValue(ThumbnailProperty) as Thumbnail;
        }

        /// <summary>
        /// Attempts to create a BitmapSource object which represents the Picture object
        /// Sets Image.Source property to the created BitmapSource object if possible
        /// Sets Image.Source property to the given thumbnail otherwise
        /// </summary>
        public static readonly DependencyProperty ThumbnailProperty =
            DependencyProperty.RegisterAttached(
                "Thumbnail",
                typeof(Thumbnail),
                typeof(PictureBitmap),
                new PropertyMetadata(OnThumbnailChanged));

        private static void OnThumbnailChanged(DependencyObject owner,
            DependencyPropertyChangedEventArgs args)
        {
            SetImageSource(owner as Image, args.NewValue as Thumbnail);
        }

        private static void SetImageSource(Image image, Thumbnail thumbnail)
        {
            if (image == null) return;

            image.Source = GetImageSource(thumbnail);
        }

        #endregion

        /// <summary>
        /// Attempts to create a BitmapSource object for the Picture thumbnail if possible
        /// Returns the thumbnail placeholder otherwise
        /// </summary>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        private static ImageSource GetImageSource(Thumbnail thumbnail)
        {
            var imageSource = GetImageSource(thumbnail.Picture);

            return imageSource ?? thumbnail.Placeholder;
        }

        /// <summary>
        /// Creates a BitmapSource object that represents the given picture object
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        private static BitmapSource GetImageSource(Picture picture)
        {
            if (picture == null) return null;

            var imageSource = PictureBitmapCache.Instance.Get(picture);

            if (imageSource == null)
            {
                imageSource = GetImageSource(PictureStore.LoadPicture(picture));
                PictureBitmapCache.Instance.Add(picture, imageSource);
            }

            return imageSource;
        }

        /// <summary>
        /// Creates a BitmapSource object from the given byte array
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static BitmapSource GetImageSource(byte[] bytes)
        {
            return ImageUtils.CreateBitmapFromBytes(bytes);
        }
    }
}

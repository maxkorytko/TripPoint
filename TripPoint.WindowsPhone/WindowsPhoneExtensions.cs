using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace TripPoint.WindowsPhone
{
    /// <summary>
    /// Placeholder for a set of extension methods
    /// </summary>
    public static class WindowsPhoneExtensions
    {
        /// <summary>
        /// Gets a query string parameter from the given page
        /// This is a utility method
        /// </summary>
        /// <param name="page"></param>
        /// <param name="parameter"></param>
        /// <returns>parameter value or an empty string</returns>
        public static string TryGetQueryStringParameter(this PhoneApplicationPage page, string parameter)
        {
            var queryString = page.NavigationContext.QueryString;
            return queryString.ContainsKey(parameter) ? queryString[parameter] : String.Empty;
        }

        /// <summary>
        /// Writes the bitmap to a stream and returns it
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Stream SaveJpeg(this WriteableBitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
            }
            catch
            {
                // nothing
            }
            finally
            {
                stream.Position = 0;
            }

            return stream;
        }

        /// <summary>
        /// Convenience method for adding items to a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="to"></param>
        /// <param name="from"></param>
        public static void Add<T>(this ICollection<T> to, IEnumerable<T> from)
        {
            if (from == null) return;

            foreach (T item in from) to.Add(item);
        }
    }
}

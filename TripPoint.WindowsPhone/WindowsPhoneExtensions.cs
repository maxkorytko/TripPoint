using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

using TripPoint.Model.Utils;

namespace TripPoint.WindowsPhone
{
    public static class WindowsPhoneExtensions
    {
        public static string TryGetQueryStringParameter(this PhoneApplicationPage page, string parameterName)
        {
            try
            {
                var value = page.NavigationContext.QueryString[parameterName];
                return value;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("TryGetQueryStringParameter for parameter '{0}' has failed. {1}",
                    parameterName, ex.Message));

                return string.Empty;
            }
        }

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

        public static void Add<T>(this ICollection<T> to, IEnumerable<T> from)
        {
            if (from == null) return;

            foreach (T item in from)
            {
                to.Add(item);
            }
        }
    }
}

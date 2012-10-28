using System;
using System.IO;

namespace TripPoint.Model.Utils
{
    /// <summary>
    /// Utility class for converting between various types
    /// </summary>
    public class TripPointConvert
    {
        /// <summary>
        /// Converts an integer to a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Empty string if conversion is not possible</returns>
        public static string ToString(int value)
        {
            string convertedValue;

            try
            {
                convertedValue = Convert.ToString(value);
            }
            catch (Exception)
            {
                convertedValue = string.Empty;
            }

            return convertedValue;
        }

        /// <summary>
        /// Converts a string to an integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns>-1 if conversion is not possible</returns>
        public static int ToInt32(string value)
        {
            int convertedValue;

            try
            {
                convertedValue = Convert.ToInt32(value);
            }
            catch (Exception)
            {
                convertedValue = -1;
            }

            return convertedValue;
        }

        /// <summary>
        /// Creates a byte array and populates it with bytes from the given stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToBytes(Stream stream)
        {
            if (stream == null || !stream.CanRead)
                return new byte[0];

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}

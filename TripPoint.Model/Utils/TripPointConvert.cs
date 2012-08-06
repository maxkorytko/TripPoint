using System;

namespace TripPoint.Model.Utils
{
    public class TripPointConvert
    {
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
    }
}

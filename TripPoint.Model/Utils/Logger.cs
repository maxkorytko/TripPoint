using System;

namespace TripPoint.Model.Utils
{
    /// <summary>
    /// Provides basic funcionality to write logs to console
    /// </summary>
    public class Logger
    {
        public static void Log(string format, params object[] args)
        {
            WriteLogString(format, args);
        }

        public static void Log(object tag, string format, params object[] args)
        {
            WriteLogString(tag, format, args);
        }

        private static void WriteLogString(object tag, String format, params object[] args)
        {
            if (tag == null) return;

            WriteLogString(tag + "::" + format, args);
        }

        private static void WriteLogString(String format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }
    }
}

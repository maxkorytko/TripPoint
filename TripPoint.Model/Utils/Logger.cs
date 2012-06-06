using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TripPoint.Model.Utils
{
    public class Logger
    {
        public static void Log(String message)
        {
            WriteLogString("{0}", message);
        }

        public static void Log(string tag, string message)
        {
            WriteLogString("{0}::{1}", tag, message);
        }

        private static void WriteLogString(String format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }
    }
}

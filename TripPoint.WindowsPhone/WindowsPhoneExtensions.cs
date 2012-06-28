#region SDK Usings
using System;
using System.Windows;
using Microsoft.Phone.Controls;
#endregion

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
    }
}

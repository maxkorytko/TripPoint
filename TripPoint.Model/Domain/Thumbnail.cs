using System;
using System.Windows.Media;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// Represents a picture thumbnail
    /// </summary>
    public class Thumbnail
    {
        public Picture Picture { get; set; }

        public ImageSource Placeholder { get; set; }
    }
}

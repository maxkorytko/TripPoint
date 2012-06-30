using System;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// A note with the ability to track the time and date it was taken on
    /// </summary>
    public class Note
    {
        public string Text { get; set; }

        /// <summary>
        /// The time and date the note was creted on
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}

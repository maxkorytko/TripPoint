using System;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// A note with the ability to track the time and date it was taken on
    /// </summary>
    public class Note
    {
        public Note()
        {
            Timestamp = DateTime.Now;
        }

        public string Text { get; set; }

        /// <summary>
        /// The time and date the note was creted on
        /// </summary>
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return string.Format("'{0}' on {1}", Text, Timestamp);
        }
    }
}

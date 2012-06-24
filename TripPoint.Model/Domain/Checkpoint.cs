using System;

namespace TripPoint.Model.Domain
{
    public class Checkpoint
    {
        /// <summary>
        /// Geographical location of the checkpoint
        /// </summary>
        public GeoLocation Location { get; set; }

        /// <summary>
        /// Checkpoint creation time and date
        /// </summary>
        public DateTime Timestamp { get; set; }

        public string Title { get; set; }

        public string Notes { get; set; }

        public override string ToString()
        {
            return string.Format("{0} at {1}. Location: {2}",
                Title, Timestamp.TimeOfDay, Location);
        }
    }
}

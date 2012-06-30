using System;
using System.Collections.ObjectModel;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// Represents a checkpoint along the trip route
    /// </summary>
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

        /// <summary>
        /// A list of notes
        /// </summary>
        public ObservableCollection<Note> Notes { get; set; }

        public override string ToString()
        {
            return string.Format("{0} at {1}. Location: {2}",
                Title, Timestamp.TimeOfDay, Location);
        }
    }
}

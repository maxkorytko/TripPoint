using System;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// Represents a trip
    /// </summary>
    public class Trip
    {
        public string Name { get; set; }

        public string Place { get; set; }

        public DateTime StartDate { get; set;  }

        public DateTime EndDate { get; set; }

        public string Notes { get; set; }
    }
}

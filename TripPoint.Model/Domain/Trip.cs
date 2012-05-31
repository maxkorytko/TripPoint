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

        public DateTime StartDate { get; private set;  }

        public DateTime EndDate { get; private set; }

        public string Notes { get; set; }
    }
}

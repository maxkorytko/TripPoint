﻿using System;

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
        public DateTime TimeStamp { get; private set; }

        public String Title { get; set; }
    }
}

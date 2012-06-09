using System;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// Represents a trip
    /// </summary>
    public class Trip
    {
        public Trip()
        {
            StartDate = DateTime.Now;
        }

        public string ID
        {
            get
            {
                int hashCode = GetHashCode();
                return Convert.ToString(hashCode);
            }
        }

        public string Name { get; set; }

        public string Place { get; set; }

        public DateTime StartDate { get; set;  }

        public DateTime EndDate { get; set; }

        public string Notes { get; set; }

        /// <summary>
        /// Validates required properties
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return IsNameValid() && IsStartDateValid() && IsEndDateValid();
        }

        #region Validation
        
        private bool IsNameValid()
        {
            return !string.IsNullOrEmpty(Name);
        }

        private bool IsStartDateValid()
        {
            return StartDate <= DateTime.Now;
        }

        private bool IsEndDateValid()
        {
            // TODO: implement
            return true;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Name: {0}, StartDate: {1}", Name, StartDate);
        }
    }
}

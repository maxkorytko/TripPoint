using System;
using System.Collections.ObjectModel;

namespace TripPoint.Model.Domain
{
    /// <summary>
    /// Represents a trip
    /// </summary>
    public class Trip
    {
        public Trip()
        {
            Name = string.Empty;
            StartDate = DateTime.Now;
            EndDate = null;
            Notes = string.Empty;
            Checkpoints = new ObservableCollection<Checkpoint>();
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

        public DateTime? EndDate { get; set; }

        public string Notes { get; set; }

        public ObservableCollection<Checkpoint> Checkpoints { get; private set; }

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

        public override int GetHashCode()
        {
            string value = string.Format("{0}{1}{2}{3}", Name, StartDate, EndDate, Notes);
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, StartDate: {1}", Name, StartDate);
        }
    }
}

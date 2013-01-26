using System;
using System.Text;
using System.Collections.Generic;

namespace TripPoint.Model.Domain.Validation
{
    /// <summary>
    /// Represents a result of performing data validation
    /// </summary>
    public class ValidationResult
    {
        private readonly IList<ValidationError> _validationErrors = new List<ValidationError>();
        private readonly StringBuilder _errorMessages = new StringBuilder();

        public bool Failed
        {
            get { return _validationErrors.Count > 0; }
        }

        public void AddValidationError(ValidationError error)
        {
            if (error == null) return;

            _validationErrors.Add(error);
            AppendErrorMessage(error.Message);
        }

        private void AppendErrorMessage(string message)
        {
            if (String.IsNullOrWhiteSpace(message)) return;

            if (_errorMessages.Length > 0) _errorMessages.AppendLine();

            _errorMessages.Append(message);
        }

        public string ErrorMessages
        {
            get
            {
                return _errorMessages.ToString();
            }
        }
    }

    public class ValidationError
    {
        public ValidationError(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}

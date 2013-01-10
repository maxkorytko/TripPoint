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
        }

        public string ErrorMessages
        {
            get
            {
                _errorMessages.Clear();
                AppendAllErrorMessages();

                return _errorMessages.ToString();
            }
        }

        private void AppendAllErrorMessages()
        {
            if (_validationErrors.Count == 0) return;

            _errorMessages.Append(_validationErrors[0].Message);

            for (int i = 1; i < _validationErrors.Count; i++)
            {
                _errorMessages.AppendLine();
                _errorMessages.Append(_validationErrors[i].Message);
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

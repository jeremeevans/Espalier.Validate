using System;
using System.Collections.Generic;
using System.Linq;

namespace Espalier.Validate
{
    public class EspalierValidationException : Exception
    {
        public EspalierValidationException(IEnumerable<ValidationError> validationErrors)
        {
            ValidationErrors = validationErrors.ToArray();
        }

        public EspalierValidationException(params string[] unattachedMessages)
        {
            var errors = new List<ValidationError>();
            errors.Add(new ValidationError(unattachedMessages));
            ValidationErrors = errors.ToArray();
        }

        public ValidationError[] ValidationErrors { get; set; }
    }
}
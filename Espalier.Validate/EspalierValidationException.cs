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

        public ValidationError[] ValidationErrors { get; set; }
    }
}
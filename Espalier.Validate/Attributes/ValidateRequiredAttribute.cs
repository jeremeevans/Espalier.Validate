﻿using System.Threading.Tasks;
using Espalier.Validate.Validations;

namespace Espalier.Validate.Attributes
{
    public class ValidateRequiredAttribute : ValidateAttribute
    {
        public override Task<string> GetError(object value, string propertyFriendlyName)
        {
            return RequiredValidation.Instance.RunValidation(value, propertyFriendlyName);
        }
    }
}
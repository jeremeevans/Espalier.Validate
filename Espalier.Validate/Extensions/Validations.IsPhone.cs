using Espalier.Validate.Validations;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Espalier.Validate.Extensions
{
    public static partial class Validations
    {
        public static ValidationContext IsPhoneNumber<TModel>(this TModel toValidate, Expression<Func<TModel, object>> selector, string friendlyName = null)
        {
            var property = (PropertyInfo)((MemberExpression)selector.Body).Member;

            return new ValidationContext
            {
                FriendlyName = string.IsNullOrWhiteSpace(friendlyName) ? property.Name : friendlyName,
                PropertyName = property.Name,
                Validation = PhoneNumberValidation.Instance,
                Value = property.GetValue(toValidate)
            };
        }
    }
}
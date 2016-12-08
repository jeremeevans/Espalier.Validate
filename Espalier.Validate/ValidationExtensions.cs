using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Espalier.Validate.Attributes;
using System.Linq.Expressions;
using Espalier.Validate.Validations;

namespace Espalier.Validate
{
    public static class ValidationExtensions
    {
        private static readonly Dictionary<Type, Dictionary<PropertyInfo, Tuple<string, ValidateAttribute[]>>> KnownModels = new Dictionary<Type, Dictionary<PropertyInfo, Tuple<string, ValidateAttribute[]>>>();





        public static async Task<ValidationError[]> Validate<TModel>(this TModel toValidate, ErrorResponse response = ErrorResponse.ThrowException)
            where TModel : class
        {
            var modelType = typeof(TModel);
            var validationErrors = new List<ValidationError>();
            Dictionary<PropertyInfo, Tuple<string, ValidateAttribute[]>> propertyValidations;

            if (!KnownModels.ContainsKey(modelType))
            {
                propertyValidations = new Dictionary<PropertyInfo, Tuple<string, ValidateAttribute[]>>();

                foreach (var property in modelType.GetRuntimeProperties())
                {
                    var validations = new List<ValidateAttribute>();

                    foreach (var validationAttribute in property.GetCustomAttributes<ValidateAttribute>())
                    {
                        validations.Add(validationAttribute);
                    }

                    var friendlyNameAttribute = property.GetCustomAttribute<FriendlyNameAttribute>();
                    propertyValidations.Add(property, new Tuple<string, ValidateAttribute[]>(friendlyNameAttribute == null ? property.Name : friendlyNameAttribute.FriendlyName, validations.ToArray()));
                }

                lock (KnownModels)
                {
                    if (!KnownModels.ContainsKey(modelType))
                    {
                        KnownModels.Add(modelType, propertyValidations);
                    }
                }
            }
            else
            {
                propertyValidations = KnownModels[modelType];
            }

            foreach (var property in propertyValidations)
            {
                var propertyValue = property.Key.GetValue(toValidate);
                var getErrorsTasks = property.Value.Item2.AsParallel()
                    .Select(validation => validation.GetError(propertyValue, property.Value.Item1));

                await Task.WhenAll(getErrorsTasks);

                var errors = getErrorsTasks
                    .Select(task => task.Result)
                    .Where(error => !string.IsNullOrWhiteSpace(error))
                    .Distinct().ToArray();

                if (errors.Any())
                {
                    validationErrors.Add(
                        new ValidationError
                        {
                            PropertyName = property.Key.Name,
                            ErrorMessages = errors
                        });
                }
            }

            switch (response)
            {
                case ErrorResponse.ThrowException:
                    if (validationErrors.Any())
                    {
                        throw new EspalierValidationException(validationErrors);
                    }
                    return new ValidationError[0];
                case ErrorResponse.ValidationErrors:
                    return validationErrors.ToArray();
                default:
                    throw new ArgumentException("Invalid error response type.");
            }
        }
    }
}
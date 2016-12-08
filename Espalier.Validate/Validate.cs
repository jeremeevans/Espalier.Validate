using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Espalier.Validate
{
    public static class Validate
    {
        public static async Task That(params ValidationContext[] contexts)
        {
            var validationTasks = contexts.AsParallel().Select(async context =>
            {
                context.ErrorFound = await context.Validation.RunValidation(context.Value, context.FriendlyName);
            }).ToArray();

            await Task.WhenAll(validationTasks);

            var errorsFound = new Dictionary<string, List<string>>();

            foreach (var context in contexts)
            {
                if (string.IsNullOrWhiteSpace(context.ErrorFound))
                {
                    continue;
                }

                if (!errorsFound.ContainsKey(context.PropertyName))
                {
                    errorsFound.Add(context.PropertyName, new List<string>());
                }

                errorsFound[context.PropertyName].Add(context.ErrorFound);
            }

            var errors = errorsFound.Select(kvp => new ValidationError
            {
                PropertyName = kvp.Key,
                ErrorMessages = kvp.Value.ToArray()
            }).ToArray();

            if(errors.Any())
            {
                throw new EspalierValidationException(errors);
            }
        }
    }
}
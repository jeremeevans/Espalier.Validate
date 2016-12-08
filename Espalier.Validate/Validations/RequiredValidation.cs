using System.Threading.Tasks;

namespace Espalier.Validate.Validations
{
    internal class RequiredValidation : IValidation
    {
        private const string RequiredErrorMessage = "{0} is required.";
        private static RequiredValidation _instance;

        private RequiredValidation()
        {
        }

        public static RequiredValidation Instance => _instance ?? (_instance = new RequiredValidation());

        public Task<string> RunValidation(object value, string propertyFriendlyName)
        {
            var stringValue = value as string;
            return Task.FromResult(string.IsNullOrWhiteSpace(stringValue) ? string.Format(RequiredErrorMessage, propertyFriendlyName) : string.Empty);
        }
    }
}
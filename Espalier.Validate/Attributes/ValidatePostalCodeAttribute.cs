using System.Threading.Tasks;
using Espalier.Validate.Validations;

namespace Espalier.Validate.Attributes
{
    public class ValidatePostalCodeAttribute : ValidateAttribute
    {
        public override Task<string> GetError(object value, string propertyFriendlyName)
        {
            return USPostalCodeValidation.Instance.RunValidation(value, propertyFriendlyName);
        }
    }
}
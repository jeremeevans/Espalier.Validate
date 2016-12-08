using System.Threading.Tasks;
using Espalier.Validate.Validations;

namespace Espalier.Validate.Attributes
{
    public class ValidatePhoneNumberAttribute : ValidateAttribute
    {
        public override Task<string> GetError(object value, string propertyFriendlyName)
        {
            return PhoneNumberValidation.Instance.RunValidation(value, propertyFriendlyName);
        }
    }
}
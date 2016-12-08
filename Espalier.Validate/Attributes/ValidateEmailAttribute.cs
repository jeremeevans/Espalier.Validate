using System.Threading.Tasks;
using Espalier.Validate.Validations;

namespace Espalier.Validate.Attributes
{
    public class ValidateEmailAttribute : ValidateAttribute
    {
        public override Task<string> GetError(object value, string propertyFriendlyName)
        {
            return EmailValidation.Instance.RunValidation(value, propertyFriendlyName);
        }
    }
}
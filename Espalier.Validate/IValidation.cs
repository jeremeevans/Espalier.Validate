using System.Threading.Tasks;

namespace Espalier.Validate
{
    public interface IValidation
    {
        Task<string> Validate(object value, string propertyFriendlyName);
    }
}
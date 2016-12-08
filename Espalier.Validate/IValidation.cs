using System.Threading.Tasks;

namespace Espalier.Validate
{
    public interface IValidation
    {
        Task<string> RunValidation(object value, string propertyFriendlyName);
    }
}
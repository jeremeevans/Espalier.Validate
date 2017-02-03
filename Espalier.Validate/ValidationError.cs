using System.Linq;

namespace Espalier.Validate
{
    public class ValidationError
    {
        public ValidationError()
        {
        }

        public ValidationError(params string[] errorMessages)
        {
            ErrorMessages = errorMessages.ToArray();
        }

        public string PropertyName { get; set; }
        public string[] ErrorMessages { get; set; }
    }
}
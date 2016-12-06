using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Espalier.Validate.Validations
{
    public abstract class RegexValidation : IValidation
    {
        private Regex _regex;

        protected abstract string Expression { get; }
        protected abstract string ErrorMessage { get; }
        protected virtual bool IgnoreCase => true;

        public Task<string> Validate(object value, string propertyFriendlyName)
        {
            var stringValue = value as string;

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return Task.FromResult(string.Empty);
            }

            return Task.FromResult((_regex ?? (_regex = new Regex(Expression, IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None))).IsMatch(stringValue) ? string.Empty : string.Format(ErrorMessage, propertyFriendlyName));
        }
    }
}
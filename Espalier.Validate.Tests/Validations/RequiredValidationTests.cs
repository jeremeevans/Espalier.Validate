using System.Threading.Tasks;
using Espalier.Validate.Validations;
using NUnit.Framework;

namespace Espalier.Validate.Tests.Validations
{
    [TestFixture]
    public class RequiredValidationTests
    {
        private const string PropertyName = "Required field";
        private const string ErrorMessage = PropertyName + " is required.";

        [Test]
        [TestCase("", ExpectedResult = ErrorMessage)]
        [TestCase(null, ExpectedResult = ErrorMessage)]
        [TestCase("test", ExpectedResult = "")]
        public async Task<string> RequiredValidations(string value)
        {
            return await RequiredValidation.Instance.RunValidation(value, PropertyName);
        }
    }
}
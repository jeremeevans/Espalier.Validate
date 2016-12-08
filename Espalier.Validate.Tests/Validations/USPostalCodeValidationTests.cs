using System.Threading.Tasks;
using Espalier.Validate.Validations;
using NUnit.Framework;

namespace Espalier.Validate.Tests.Validations
{
    [TestFixture]
    public class USPostalCodeValidationTests
    {
        private const string PropertyName = "Postal code";
        private const string ErrorMessage = PropertyName + " is not a valid postal code.";

        [Test]
        [TestCase("98203", ExpectedResult = "")]
        [TestCase("05493", ExpectedResult = "")]
        [TestCase("05493-8348", ExpectedResult = "")]
        [TestCase("98203-1234", ExpectedResult = "")]
        [TestCase("", ExpectedResult = "")]
        [TestCase(null, ExpectedResult = "")]
        [TestCase("9820", ExpectedResult = ErrorMessage)]
        [TestCase("98203ab", ExpectedResult = ErrorMessage)]
        [TestCase("982031", ExpectedResult = ErrorMessage)]
        [TestCase("9820323235", ExpectedResult = ErrorMessage)]
        [TestCase("98203-12350", ExpectedResult = ErrorMessage)]
        public async Task<string> ValidateEmailAddress(string postalCode)
        {
            return await USPostalCodeValidation.Instance.RunValidation(postalCode, PropertyName);
        }
    }
}
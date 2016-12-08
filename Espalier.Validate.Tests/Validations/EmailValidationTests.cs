using System.Threading.Tasks;
using Espalier.Validate.Validations;
using NUnit.Framework;

namespace Espalier.Validate.Tests.Validations
{
    [TestFixture]
    public class EmailValidationTests
    {
        private const string PropertyName = "Email address";
        private const string ErrorMessage = PropertyName + " is not a valid email address.";

        [Test]
        [TestCase("", ExpectedResult = "")]
        [TestCase(null, ExpectedResult = "")]
        [TestCase("test@test.com", ExpectedResult = "")]
        [TestCase("test", ExpectedResult = ErrorMessage)]
        public async Task<string> ValidateEmailAddress(string emailAddress)
        {
            return await EmailValidation.Instance.RunValidation(emailAddress, PropertyName);
        }
    }
}
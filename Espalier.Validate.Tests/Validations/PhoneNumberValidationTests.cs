using System.Threading.Tasks;
using Espalier.Validate.Validations;
using NUnit.Framework;

namespace Espalier.Validate.Tests.Validations
{
    [TestFixture]
    public class PhoneNumberValidationTests
    {
        private const string PropertyName = "Phone number";
        private const string ErrorMessage = PropertyName + " is not a valid phone number.";

        [Test]
        [TestCase("", ExpectedResult = "")]
        [TestCase(null, ExpectedResult = "")]
        [TestCase("3849388238", ExpectedResult = "")]
        [TestCase("384 938-8238", ExpectedResult = "")]
        [TestCase("384-938-8238", ExpectedResult = "")]
        [TestCase("(384) 938-8238", ExpectedResult = "")]
        [TestCase("384.938.8238", ExpectedResult = "")]
        [TestCase("384 938 8238", ExpectedResult = "")]
        [TestCase("4444-444-234", ExpectedResult = ErrorMessage)]
        [TestCase("42594545", ExpectedResult = ErrorMessage)]
        [TestCase("4354", ExpectedResult = ErrorMessage)]
        [TestCase("834798374983928", ExpectedResult = ErrorMessage)]
        [TestCase("425 909-93488", ExpectedResult = ErrorMessage)]
        [TestCase("909-3488", ExpectedResult = ErrorMessage)]
        [TestCase("9093488", ExpectedResult = ErrorMessage)]
        public async Task<string> PhoneNumberValidateTest(string phoneNumber)
        {
            return await PhoneNumberValidation.Instance.Validate(phoneNumber, PropertyName);
        }
    }
}
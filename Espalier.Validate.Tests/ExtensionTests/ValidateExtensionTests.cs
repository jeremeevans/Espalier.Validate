using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Espalier.Validate.Tests.ExtensionTests
{
    [TestFixture]
    public class ValidateExtensionTests
    {
        private const string EmailError = " is not a valid email address.";
        private const string RequiredError = " is required.";

        [Test]
        [TestCase("test@test.com", "test@test.com", "test", "", "")]
        [TestCase("", "test@test.com", "test", "", "")]
        [TestCase(null, "test@test.com", "test", "", "")]
        [TestCase("test", "test@test.com", "test", "NotRequiredEmail", TestModel.NotRequiredEmailFriendlyName + EmailError)]
        [TestCase("", "test", "test", "RequiredEmail", TestModel.RequiredEmailFriendlyName + EmailError)]
        [TestCase("", "", "test", "RequiredEmail", TestModel.RequiredEmailFriendlyName + RequiredError)]
        [TestCase("", null, "test", "RequiredEmail", TestModel.RequiredEmailFriendlyName + RequiredError)]
        public async Task ValidateExtension(string notRequiredEmail, string requiredEmail, string requiredString, string errorField, params string[] expectedErrors)
        {
            var model = new TestModel
            {
                NotRequiredEmail = notRequiredEmail,
                RequiredEmail = requiredEmail,
                RequiredString = requiredString
            };

            var errors = await model.Validate(ErrorResponse.ValidationErrors);

            if (!string.IsNullOrWhiteSpace(errorField))
            {
                Assert.AreEqual(1, errors.Length);
                Assert.AreEqual(errors[0].PropertyName, errorField);
                Assert.AreEqual(expectedErrors.Length, errors[0].ErrorMessages.Length);
                Assert.True(expectedErrors.All(expected => errors[0].ErrorMessages.Any(message => message == expected)));
            }
            else
            {
                Assert.AreEqual(0, errors.Length);
            }
        }
    }
}
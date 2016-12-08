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

        [Test]
        public void IsPhoneNumberExtensionThrowsForInvalid()
        {
            var model = new
            {
                Phone = "983-8932"
            };

            var exception = Assert.ThrowsAsync<EspalierValidationException>(async () => await Validate.That(model.IsPhoneNumber(m => m.Phone, "Phone number")));

            Assert.AreEqual(1, exception.ValidationErrors.Length);
            Assert.AreEqual("Phone number is not a valid phone number.", exception.ValidationErrors[0].ErrorMessages[0]);
            Assert.AreEqual("Phone", exception.ValidationErrors[0].PropertyName);
        }

        [Test]
        public async Task IsPhoneNumberExtensionDoesNotThrowForValid()
        {
            var model = new
            {
                Phone = "602 983-8932"
            };

            await Validate.That(model.IsPhoneNumber(m => m.Phone, "Phone number"));
        }

        [Test]
        public void IsRequiredExtensionThrowsForInvalid()
        {
            var model = new
            {
                Phone = ""
            };

            var exception = Assert.ThrowsAsync<EspalierValidationException>(async () => await Validate.That(model.IsRequired(m => m.Phone, "Phone number")));

            Assert.AreEqual(1, exception.ValidationErrors.Length);
            Assert.AreEqual("Phone number is required.", exception.ValidationErrors[0].ErrorMessages[0]);
            Assert.AreEqual("Phone", exception.ValidationErrors[0].PropertyName);
        }

        [Test]
        public async Task IsRequiredExtensionDoesNotThrowForValid()
        {
            var model = new
            {
                Phone = "602 983-8932"
            };

            await Validate.That(model.IsRequired(m => m.Phone, "Phone number"));
        }

        [Test]
        public void IsEmailExtensionThrowsForInvalid()
        {
            var model = new
            {
                Email = "test"
            };

            var exception = Assert.ThrowsAsync<EspalierValidationException>(async () => await Validate.That(model.IsEmail(m => m.Email, "Email address")));

            Assert.AreEqual(1, exception.ValidationErrors.Length);
            Assert.AreEqual("Email address is not a valid email address.", exception.ValidationErrors[0].ErrorMessages[0]);
            Assert.AreEqual("Email", exception.ValidationErrors[0].PropertyName);
        }

        [Test]
        public async Task IsEmailExtensionDoesNotThrowForValid()
        {
            var model = new
            {
                Email = "test@test.com"
            };

            await Validate.That(model.IsEmail(m => m.Email, "Email address"));
        }

        [Test]
        public void IsUSPostalCodeThrowsForInvalid()
        {
            var model = new
            {
                Zip = "843"
            };

            var exception = Assert.ThrowsAsync<EspalierValidationException>(async () => await Validate.That(model.IsUSPostalCode(m => m.Zip, "Zip code")));

            Assert.AreEqual(1, exception.ValidationErrors.Length);
            Assert.AreEqual("Zip code is not a valid postal code.", exception.ValidationErrors[0].ErrorMessages[0]);
            Assert.AreEqual("Zip", exception.ValidationErrors[0].PropertyName);
        }

        [Test]
        public async Task IsUSPostalCodeDoesNotThrowForValid()
        {
            var model = new
            {
                Zip = "85020"
            };

            await Validate.That(model.IsUSPostalCode(m => m.Zip, "Zip code"));
        }

        [Test]
        public void ValidateMultipleHasCorrectNumberOfErrors()
        {
            var model = new
            {
                Zip = "843",
                Phone = "ABC",
                Required = ""
            };

            var exception = Assert.ThrowsAsync<EspalierValidationException>(async () => 
                await Validate.That(
                    model.IsUSPostalCode(m => m.Zip, "Zip code"),
                    model.IsPhoneNumber(m => m.Zip, "Zip code"),
                    model.IsRequired(m => m.Zip, "Zip code"),
                    model.IsPhoneNumber(m => m.Phone, "Phone number"),
                    model.IsRequired(m => m.Required, "Required field")));

            Assert.AreEqual(3, exception.ValidationErrors.Length);
            var zipErrors = exception.ValidationErrors.First(errors => errors.PropertyName == "Zip");
            Assert.AreEqual(2, zipErrors.ErrorMessages.Length);
            Assert.True(zipErrors.ErrorMessages.Any(error => error == "Zip code is not a valid postal code."));
            Assert.True(zipErrors.ErrorMessages.Any(error => error == "Zip code is not a valid phone number."));
            var phoneErrors = exception.ValidationErrors.First(errors => errors.PropertyName == "Phone");
            Assert.AreEqual(1, phoneErrors.ErrorMessages.Length);
            Assert.AreEqual("Phone number is not a valid phone number.", phoneErrors.ErrorMessages[0]);
            var requiredErrors = exception.ValidationErrors.First(errors => errors.PropertyName == "Required");
            Assert.AreEqual(1, requiredErrors.ErrorMessages.Length);
            Assert.AreEqual("Required field is required.", requiredErrors.ErrorMessages[0]);
        }
    }
}
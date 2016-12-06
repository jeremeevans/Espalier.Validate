using Espalier.Validate.Attributes;

namespace Espalier.Validate.Tests.ExtensionTests
{
    public class TestModel
    {
        public const string NotRequiredEmailFriendlyName = "Not required email";
        public const string RequiredEmailFriendlyName = "Required email";
        public const string RequiredStringFriendlyName = "Required string";

        [ValidateEmail]
        [FriendlyName(NotRequiredEmailFriendlyName)]
        public string NotRequiredEmail { get; set; }

        [ValidateEmail]
        [ValidateRequired]
        [FriendlyName(RequiredEmailFriendlyName)]
        public string RequiredEmail { get; set; }

        [ValidateRequired]
        [FriendlyName(RequiredStringFriendlyName)]
        public string RequiredString { get; set; }
    }
}
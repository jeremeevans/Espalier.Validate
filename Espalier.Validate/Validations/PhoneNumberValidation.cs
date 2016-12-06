namespace Espalier.Validate.Validations
{
    public class PhoneNumberValidation : RegexValidation
    {
        private const string PhoneNumberExpression = @"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$";
        private const string PhoneNumberMessage = "{0} is not a valid phone number.";
        private static PhoneNumberValidation _instance;

        private PhoneNumberValidation()
        {
        }

        protected override string Expression => PhoneNumberExpression;
        protected override string ErrorMessage => PhoneNumberMessage;

        public static PhoneNumberValidation Instance => _instance ?? (_instance = new PhoneNumberValidation());
    }
}
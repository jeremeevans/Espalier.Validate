namespace Espalier.Validate.Validations
{
    public class USPostalCodeValidation : RegexValidation
    {
        // TODO: Refactor this to have a Postal Code flags enum that specifies valid postal code types.
        private const string USPostalCodeExpression = @"^(?!00000)(?<zip>(?<zip5>\d{5})(?:[ -](?=\d))?(?<zip4>\d{4})?)$";
        private const string USPostalCodeErrorMessage = "{0} is not a valid postal code.";
        private static USPostalCodeValidation _instance;

        private USPostalCodeValidation()
        {
        }

        protected override string Expression => USPostalCodeExpression;
        protected override string ErrorMessage => USPostalCodeErrorMessage;

        public static USPostalCodeValidation Instance => _instance ?? (_instance = new USPostalCodeValidation());
    }
}
namespace Espalier.Validate
{
    public class ValidationContext
    {
        public object Value { get; set; }
        public string FriendlyName { get; set; }
        public string PropertyName { get; set; }
        public IValidation Validation { get; set; }
        public string ErrorFound { get; set; }
    }
}
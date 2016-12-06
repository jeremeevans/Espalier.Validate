namespace Espalier.Validate
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string[] ErrorMessages { get; set; }
    }
}
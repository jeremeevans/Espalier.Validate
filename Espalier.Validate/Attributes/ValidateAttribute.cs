using System;
using System.Threading.Tasks;

namespace Espalier.Validate.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidateAttribute : Attribute
    {
        public abstract Task<string> GetError(object value, string propertyFriendlyName);
    }
}
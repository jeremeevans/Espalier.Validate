[![espalier-validate MyGet Build Status](https://www.myget.org/BuildSource/Badge/espalier-validate?identifier=46603932-4c46-4d55-9ce5-b5bcb628ac47)](https://www.myget.org/)

# Espalier.Validate

Validation attributes, fluent interface, and extensions to validate models and throw rich exceptions.

## Goals of Espalier.Validate

* Create a .NET validation library with minimal dependencies.
* Make it provide a lot of detail as to why a given validation error was thrown.
* Provide sensible default validations.
* It should be easily extendable.
* Allow consumption via attributes, or a more direct approach.
* Push validation down into services and the domain to keep code DRY.
* Things should run async and in parallel whenever possible to make it fast.

## Using the library

### To return a JSON payload with errors from your Web API 2 project

[Espalier.Validate.WebAPI2](//github.com/jeremeevans/Espalier.Validate.WebAPI2) has an exception handler that will create HTTP 400 responses with a JSON payload containing all the errors Espalier.Validate generates. Check it out!

### When you have a well defined model structure up front

If you have a defined set of models up front, the best way to use Espalier.Validate is via validation attributes. If you look at the [TestModel class in Espalier.Validate.Tests](//github.com/jeremeevans/Espalier.Validate/blob/master/Espalier.Validate.Tests/ExtensionTests/TestModel.cs) you can see how to use the attributes:

```csharp
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
```

Then you can validate an instance of that model using the Validate extension:

```csharp
var model = new TestModel
{
    NotRequiredEmail = "bad email",
    RequiredEmail = "",
    RequiredString = "I am required."
};

await model.Validate();
```

In this case, an EspalierValidationException will be thrown with the errors that were encountered. If you would rather receive an array of validation errors and do whatever you want with them, you can call Validate as such:

```csharp
var model = new TestModel
{
    NotRequiredEmail = "bad email",
    RequiredEmail = "",
    RequiredString = "I am required."
};

var errors = await model.Validate(ErrorResponse.ValidationErrors);
```

### When you do not have a defined structure or do not want to decorate your classes

Espalier.Validate also provides extensions to help you validate the properties of any object. To validate properties on the fly, use Validate.That and pass in as many ValidationContext instances as you like. There are extension methods for creating ValidationContext instances for the built-in validations. Here is an example:

```csharp
var model = new
{
    Zip = "ABC",
    Phone = "394 934-9348",
    Required = ""
};

await Validate.That(
    model.IsUSPostalCode(m => m.Zip, "Zip code"),
    model.IsRequired(m => m.Zip, "Zip code"),
    model.IsPhoneNumber(m => m.Phone, "Phone number"),
    model.IsRequired(m => m.Required, "Required field"));
```

Validate.That will throw an EspalierValidationException containing all the validation errors that were found within the ValidationContext instances provided.

## Validations currently in the project

There are several handy validations currently in the project. If there are more that you need, you can either fork the project, build them, and submit a pull request, or [email me](mailto:jereme@jeremeevans.com) and I will probably build them for you. Here are the current validations:

* Email
 - ValidateEmailAttribute
 - model.IsEmail
* Phone Number (currently USA style phone number only)
 - ValidatePhoneNumberAttribute
 - model.IsPhoneNumber
* Required
 - ValidateRequiredAttribute
 - model.IsRequired
* US Postal Code
 - ValidatePostalCodeAttribute
 - model.IsUSPostalCode

## How to create your own validations

The current set of validations is clearly not a comprehensive list of the validations that could exist. In addition to that, you may have some unique scenarios. To create your own validations, implement the IValidation interface, and then implement a ValidateAttribute or create a way to build a ValidationContext instance for it. Here is the implementation of the RequiredValidation:

```csharp
internal class RequiredValidation : IValidation
{
    private const string RequiredErrorMessage = "{0} is required.";
    private static RequiredValidation _instance;

    private RequiredValidation()
    {
    }

    public static RequiredValidation Instance => _instance ?? (_instance = new RequiredValidation());

    public Task<string> RunValidation(object value, string propertyFriendlyName)
    {
        var stringValue = value as string;
        return Task.FromResult(string.IsNullOrWhiteSpace(stringValue) ? string.Format(RequiredErrorMessage, propertyFriendlyName) : string.Empty);
    }
}

public class ValidateRequiredAttribute : ValidateAttribute
{
    public override Task<string> GetError(object value, string propertyFriendlyName)
    {
        return RequiredValidation.Instance.RunValidation(value, propertyFriendlyName);
    }
}

public static class ValidationExtensions
{
    private static readonly Dictionary<Type, Dictionary<PropertyInfo, Tuple<string, ValidateAttribute[]>>> KnownModels = new Dictionary<Type, Dictionary<PropertyInfo, Tuple<string, ValidateAttribute[]>>>();

    public static ValidationContext IsRequired<TModel>(this TModel toValidate, Expression<Func<TModel, object>> selector, string friendlyName = null)
    {
        var property = (PropertyInfo)((MemberExpression)selector.Body).Member;

        return new ValidationContext
        {
            FriendlyName = string.IsNullOrWhiteSpace(friendlyName) ? property.Name : friendlyName,
            PropertyName = property.Name,
            Validation = RequiredValidation.Instance,
            Value = property.GetValue(toValidate)
        };
    }
}
```

## Contributing

I would love to have your contributions to this library! If you create some validations that other people may appreciate, or have some ideas to improve the library, please let me know. Then go ahead and fork the repository, add your validations, and submit a pull request.
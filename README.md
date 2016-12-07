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

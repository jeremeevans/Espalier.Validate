@echo Off

dotnet restore .\Espalier.Validate.Tests\project.json
dotnet restore .\Espalier.Validate\project.json

dotnet test .\Espalier.Validate.Tests\
if not "%errorlevel%"=="0" goto failure

dotnet build .\Espalier.Validate\project.json --configuration Release
if not "%errorlevel%"=="0" goto failure

dotnet pack .\Espalier.Validate\project.json --configuration release

:success
exit 0

:failure
exit -1
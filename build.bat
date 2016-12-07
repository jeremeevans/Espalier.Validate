@echo Off

set version=0.0.1
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

set nunit=".\NUnit.Console-3.5.0\nunit3-console.exe"

dotnet restore .\Espalier.Validate.Tests\project.json
dotnet restore .\Espalier.Validate\project.json

dotnet test .\Espalier.Validate.Tests\
if not "%errorlevel%"=="0" goto failure

dotnet build .\Espalier.Validate\project.json --configuration Release
if not "%errorlevel%"=="0" goto failure

dotnet pack .\Espalier.Validate\project.json --configuration release

:failure
exit -1
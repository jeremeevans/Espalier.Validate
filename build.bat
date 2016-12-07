@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=0.0.1
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

set nunit=".\NUnit.Console-3.5.0\nunit3-console.exe"

dotnet restore .\Espalier.Validate\project.json
dotnet build .\Espalier.Validate\project.json --configuration %config%
if not "%errorlevel%"=="0" goto failure

dotnet restore .\Espalier.Validate.Tests\project.json
dotnet build .\Espalier.Validate.Tests\project.json --configuration %config%
if not "%errorlevel%"=="0" goto failure

%nunit% .\Espalier.Validate.Tests\bin\%config%\Espalier.Validate.Tests.dll
if not "%errorlevel%"=="0" goto failure

dotnet pack .\Espalier.Validate\project.json --configuration release

:failure
exit -1
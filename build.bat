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

dotnet restore .\Espalier.Validate\project.json
dotnet build .\Espalier.Validate\project.json --configuration Release
dotnet pack .\Espalier.Validate\project.json --configuration release
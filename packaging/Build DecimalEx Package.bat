@echo off
echo ------------------------------------
echo Building DecimalEx package...
echo ------------------------------------
if not exist "%~dp0nuget" md "%~dp0nuget"
dotnet pack "%~dp0..\DecimalEx\DecimalEx.csproj" --configuration Release -p:GenerateDocumentationFile=true --include-symbols -p:SymbolPackageFormat=snupkg --output "%~dp0nuget"
echo.
pause
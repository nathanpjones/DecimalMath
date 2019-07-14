@echo off
echo ------------------------------------
echo Building Decimal2D package...
echo ------------------------------------
if not exist "%~dp0nuget" md "%~dp0nuget"
dotnet pack "%~dp0..\Decimal2D\Decimal2D.csproj" --configuration Release --include-symbols --output "%~dp0nuget"
echo.
pause
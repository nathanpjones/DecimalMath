@echo off
echo ------------------------------------
echo Building Decimal2D package...
echo ------------------------------------
if not exist "%~dp0nuget" md "%~dp0nuget"
nuget pack "%~dp0..\Decimal2D\Decimal2D.csproj" -build -Prop Configuration=Release -OutputDirectory "%~dp0nuget" -IncludeReferencedProjects -Symbols
echo.
pause
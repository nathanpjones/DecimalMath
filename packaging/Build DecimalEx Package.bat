@echo off
echo ------------------------------------
echo Building DecimalEx package...
echo ------------------------------------
if not exist "%~dp0nuget" md "%~dp0nuget"
nuget pack "%~dp0..\DecimalEx\DecimalEx.csproj" -build -Prop Configuration=Release -OutputDirectory "%~dp0nuget" -Symbols
echo.
pause
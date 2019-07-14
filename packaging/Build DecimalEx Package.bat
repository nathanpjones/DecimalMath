@echo off
echo ------------------------------------
echo Building DecimalEx package...
echo ------------------------------------
if not exist "%~dp0nuget" md "%~dp0nuget"
dotnet pack "%~dp0..\DecimalEx\DecimalEx.csproj" --configuration Debug --output "%~dp0nuget" --include-symbols /p:PackageVersion=1.0.1 "/p:Authors=Nathan P Jones" "/p:Owner=Nathan P Jones" /p:PackageLicenseUrl=https://github.com/nathanpjones/DecimalMath/blob/master/LICENSE /p:PackageProjectUrl=https://github.com/nathanpjones/DecimalMath /p:PackageRequireLicenseAcceptance=false "/p:Description=Portable math support for Decimal that Microsoft forgot and more. Includes Decimal versions of Sqrt Pow Exp and Log as well as the trig functions Sin Cos Tan ASin ACos ATan ATan2. Also included is other functionality for working with numbers in Decimal precision." "/p:Copyright=Copyright 2015 - Nathan P Jones" "/p:PackageTags=c# decimal .net math pcl trigonometry
echo.
pause
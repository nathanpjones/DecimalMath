@echo off
echo ------------------------------------
echo Building Decimal2D package...
echo ------------------------------------
if not exist "%~dp0nuget" md "%~dp0nuget"
dotnet pack "%~dp0..\Decimal2D\Decimal2D.csproj" --configuration Debug --output "%~dp0nuget" --include-symbols /p:PackageVersion=1.0.1 "/p:Authors=Nathan P Jones" "/p:Owner=Nathan P Jones" /p:PackageLicenseUrl=https://github.com/nathanpjones/DecimalMath/blob/master/LICENSE /p:PackageProjectUrl=https://github.com/nathanpjones/DecimalMath /p:PackageRequireLicenseAcceptance=false "/p:Description=Portable math support for Decimal-based geometric and trigonometric calculations." "/p:Copyright=Copyright 2015 - Nathan P Jones" "/p:PackageTags=c# decimal .net math pcl trigonometry geometry"
echo.
pause
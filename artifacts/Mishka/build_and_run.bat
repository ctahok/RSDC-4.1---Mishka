@echo off
cd /d "%~dp0"
echo Checking .NET SDK...
dotnet --version
if errorlevel 1 (
    echo ERROR: .NET SDK not found. Please install .NET 8 SDK from https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)
echo Building Mishka...
dotnet publish Mishka.csproj -c Release -r win-x64 -o "..\output"
if errorlevel 1 (
    echo ERROR: Build failed.
    pause
    exit /b 1
)
echo.
echo Build complete!
echo Output: %~dp0..\output\Mishka.exe
pause
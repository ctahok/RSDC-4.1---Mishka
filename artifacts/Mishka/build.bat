@echo off
echo ==========================================
echo Mishka - Build Script
echo ==========================================
echo.

:: Check for .NET SDK
where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK not found.
    echo Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0
    exit /b 1
)

echo Building Mishka... (framework-dependent, ~1-3 MB)
echo.

:: Build the project
dotnet publish Mishka.csproj -c Release -o ../output --self-contained false -r win-x64 /p:PublishSingleFile=true

if %errorlevel% neq 0 (
    echo ERROR: Build failed
    exit /b 1
)

echo.
echo ==========================================
echo Build Complete!
echo ==========================================
echo.
echo Output: output/mishka.exe (~1-3 MB)
echo.
echo IMPORTANT: Requires .NET 8 Runtime to be installed
echo Download from: https://dotnet.microsoft.com/download/dotnet/8.0
echo.
pause
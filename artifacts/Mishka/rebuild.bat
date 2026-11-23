@echo off
cd /d "%~dp0"

echo Killing any running Mishka instances...
taskkill /F /IM Mishka.exe >nul 2>&1
timeout /t 2 /nobreak >nul

echo Cleaning output directory...
if exist "..\output\Mishka.exe" del /f /q "..\output\Mishka.exe"
if exist "..\output" rd /s /q "..\output"
if exist "obj" rd /s /q "obj"
if exist "bin" rd /s /q "bin"

echo Building Mishka...
"C:\Program Files\dotnet\dotnet.exe" publish Mishka.csproj -c Release -r win-x64 -o "..\output"
if errorlevel 1 (
    echo ERROR: Build failed.
    pause
    exit /b 1
)

echo.
echo Build complete!
echo Output: %~dp0..\output\Mishka.exe
pause
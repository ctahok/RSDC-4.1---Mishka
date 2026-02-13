# Mishka Build Script with PowerShell Module Support
param(
    [switch]$InstallPSModule,
    [switch]$SelfContained,
    [switch]$Run,
    [switch]$Clean,
    [switch]$Install
)

$ErrorActionPreference = "Stop"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Mishka - Enhanced Build Script" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

if ($Clean) {
    Write-Host "Cleaning previous build artifacts..." -ForegroundColor Green
    if (Test-Path "obj") { Remove-Item -Path "obj" -Recurse -Force }
    if (Test-Path "bin") { Remove-Item -Path "bin" -Recurse -Force }
    if (Test-Path "publish") { Remove-Item -Path "publish" -Recurse -Force }
    Write-Host "Clean completed." -ForegroundColor Green
    return
}

# Install PowerShell Module if requested
if ($InstallPSModule) {
    Write-Host "Installing PowerShell module..." -ForegroundColor Green
    
    try {
        $modulesPath = Join-Path $env:USERPROFILE "Documents\PowerShell\Modules"
        $moduleSource = Resolve-Path (Join-Path $PSScriptRoot "..\..\.tmp\MouseJigglerPS")
        $moduleName = "MouseJiggler"
        $modulePath = Join-Path $modulesPath $moduleName
        
        # Check if source module exists
        if (Test-Path $moduleSource) {
            # Create parent directory if needed
            if (-not (Test-Path $modulesPath)) {
                Write-Host "Creating modules directory: $modulesPath" -ForegroundColor Gray
                New-Item -ItemType Directory -Force -Path $modulesPath | Out-Null
            }
            
            # Install module
            Write-Host "Installing module to $modulePath..." -ForegroundColor Gray
            if (Test-Path $modulePath) { Remove-Item -Path $modulePath -Recurse -Force }
            
            # Simple Copy-Item of the directory
            Copy-Item -Path $moduleSource -Destination $modulePath -Recurse -Force
            Write-Host "PowerShell module installed successfully." -ForegroundColor Green
        } else {
            Write-Host "WARNING: PowerShell module source not found at $moduleSource" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "WARNING: Failed to install PowerShell module: $($_.Exception.Message)" -ForegroundColor Yellow
        Write-Host "Continuing with build..." -ForegroundColor Gray
    }
}

# Determine output directory
if ($Install) {
    $outputDir = Join-Path ${env:ProgramFiles} "Mishka"
} else {
    $outputDir = Join-Path $PSScriptRoot "..\output"
}

# Build the project
Write-Host "Building Mishka..." -ForegroundColor White

$projectFile = "Mishka.csproj"

if ($SelfContained) {
    Write-Host "Building Self-contained version..." -ForegroundColor Green
    & dotnet publish $projectFile -c Release -r win-x64 --self-contained true -o $outputDir
} else {
    Write-Host "Building Framework-dependent version..." -ForegroundColor Green
    & dotnet publish $projectFile -c Release -r win-x64 -o $outputDir
}

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Build failed" -ForegroundColor Red
    exit 1
}

Write-Host "Build completed successfully!" -ForegroundColor Green

# Display file info
$exePath = Join-Path $outputDir "Mishka.exe"
if (Test-Path $exePath) {
    $fileInfo = Get-Item $exePath
    $size = $fileInfo.Length / 1MB
    Write-Host "Created: $exePath" -ForegroundColor Green
    Write-Host "Size: $($size.ToString('F2')) MB" -ForegroundColor Cyan
    Write-Host "Requires .NET 8 Runtime" -ForegroundColor Yellow
}

# Run the application if requested
if ($Run -and (Test-Path $exePath)) {
    Write-Host "Starting Mishka..." -ForegroundColor Green
    Start-Process -FilePath $exePath
    Write-Host "Mishka started successfully!" -ForegroundColor Green
}

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Build script completed." -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

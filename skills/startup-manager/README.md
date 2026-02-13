---
name: startup-manager
description: Manage Windows auto-startup via registry entries for applications
version: 1.0
---

# Startup Manager

## Usage
Use this skill when implementing auto-startup functionality for Windows applications. Manages registry entries in HKCU\Software\Microsoft\Windows\CurrentVersion\Run.

## Instructions
1. Run `generate_startup_manager.py` to create the startup manager class
2. Call EnableStartup() to add app to startup
3. Call DisableStartup() to remove from startup
4. Call IsStartupEnabled() to check current status

## Tools (Scripts)
*   **Generate Startup Manager:** `python scripts/generate_startup_manager.py --output <path> --app-name <name>`

## Dependencies
*   Python 3.8+
*   Windows OS (uses Microsoft.Win32.Registry)
*   .NET 6+ or .NET Framework

## Registry Location
HKCU\Software\Microsoft\Windows\CurrentVersion\Run

## Security Note
Requires user consent. Never enable startup without explicit user action.
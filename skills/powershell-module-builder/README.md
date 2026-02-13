---
name: powershell-module-builder
description: Create PowerShell modules with cmdlets for CLI automation
version: 1.0
---

# PowerShell Module Builder

## Usage
Use this skill when creating PowerShell modules for command-line interfaces. Generates module manifests, script modules, and installation helpers.

## Instructions
1. Run `create_module.py` to generate module structure
2. Edit the .psm1 file to add functions
3. Run `install_module.py` to install to PowerShell modules path

## Tools (Scripts)
*   **Create Module:** `python scripts/create_module.py --name <module_name> --output <path> [--functions <func1,func2>]`
*   **Install Module:** `python scripts/install_module.py --path <module_dir> [--scope <CurrentUser|AllUsers>]`

## Dependencies
*   Python 3.8+
*   PowerShell 5.1 or PowerShell Core 7+
*   Windows PowerShell or PowerShell
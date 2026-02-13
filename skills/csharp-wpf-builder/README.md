---
name: csharp-wpf-builder
description: Build C# WPF applications with modern minimalistic UI for Windows
version: 1.0
---

# C# WPF Builder

## Usage
Use this skill when building Windows desktop applications with modern UI requirements. Generates project scaffolding, XAML files, and build scripts for self-contained executables.

## Instructions
1. Run `create_project.py` to generate WPF project structure
2. Customize UI and logic files as needed
3. Run `build_executable.py` to compile standalone .exe

## Tools (Scripts)
*   **Create Project:** `python scripts/create_project.py --name <app_name> --output <path> [--template <minimal|standard>]`
*   **Build Executable:** `python scripts/build_executable.py --project <path> --output <dir>`
*   **Add Window:** `python scripts/add_window.py --project <path> --name <window_name>`

## Dependencies
*   Python 3.8+
*   .NET 8.0 SDK (for build step)
*   Windows 10/11
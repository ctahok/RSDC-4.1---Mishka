# Mishka: Advanced Mouse Jiggler Suite 🖱️

Mishka is a professional-grade Windows utility designed to keep your system active and prevent screen lock or sleep. It offers both a polished GUI application and a PowerShell CLI module for automation.

## 🌟 Key Features

- **Intuitive GUI**: Modern, high-visibility interface for easy configuration.
- **PowerShell CLI**: Scriptable control over mouse activity and schedules.
- **Intelligent Scheduling**: Automate jiggling based on your working hours.
- **Global Accessibility**: Use customizable hotkeys to show/hide the app instantly.
- **High Compatibility**: Supports both physical and virtual (Zen) jiggle modes.

## 📂 Project Organization

- **[Mishka GUI App](./artifacts/Mishka)**: Core WPF application with full feature set.
- **[PowerShell Module](./.tmp/MouseJigglerPS)**: Source for the CLI module.
- **[Build Artifacts](./artifacts)**: Compiled binaries and project state documentation.

## 🚀 Getting Started

To build and run the main application:

1. Navigate to the Mishka directory:
   ```powershell
   cd artifacts/Mishka
   ```
2. Run the build script:
   ```powershell
   .\build.ps1 -Run
   ```

For detailed usage, configuration, and building options, please see the **[Mishka README](./artifacts/Mishka/README.md)**.

## 📜 Requirements

- Windows 10/11
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

---
*© 2026 iliko*

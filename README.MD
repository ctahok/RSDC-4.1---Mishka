# Mishka: Advanced Mouse Jiggler Suite 🖱️ v1.3.7

**Version 1.1.1** - Minimal Movement, Maximum Reliability

Mishka is a professional-grade Windows utility designed to keep your system active and prevent screen lock or sleep. It offers both a polished GUI application and a PowerShell CLI module for automation.

## 🌟 Key Features

- **Minimal Movement**: Barely noticeable 1-pixel cursor jiggle (or invisible Zen mode)
- **Reliable Prevention**: Uses official Windows `SetThreadExecutionState` API
- **Intuitive GUI**: Modern, high-visibility interface for easy configuration
- **PowerShell CLI**: Scriptable control over mouse activity and schedules
- **Intelligent Scheduling**: Automate jiggling based on your working hours
- **Global Accessibility**: Use customizable hotkeys to show/hide the app instantly
- **Dual Modes**: Physical (1px movement) or Zen (invisible) jiggle modes

## ⚡ Quick Start

### Download Pre-built Release
1. Download `Mishka.exe` from [Releases](https://github.com/ctahok/RSDC-4.1---Mishka/releases)
2. Install [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) if needed
3. Run `Mishka.exe`
4. Check "Enable jiggle?" to start

### Build from Source
```powershell
cd artifacts/Mishka
.\build.ps1 -Run
```

## 📂 Project Organization

- **[Mishka GUI App](./artifacts/Mishka)**: Core WPF application with full feature set
- **[PowerShell Module](./.tmp/MouseJigglerPS)**: Source for the CLI module
- **[Build Artifacts](./artifacts)**: Compiled binaries and project state documentation
- **[Release Notes](./RELEASE_NOTES.md)**: Detailed changelog and version history

## 🎯 What's New in v1.1.1

- **Minimal Movement**: Reduced to 1 pixel (barely noticeable)
- **Dynamic Versioning**: About window shows current version automatically
- **Clean Build**: 0 errors, 0 warnings with full .NET 8 compliance
- **Bug Fixes**: Reliable screen lock prevention with Windows API

See [RELEASE_NOTES.md](./RELEASE_NOTES.md) for complete details.

## 🚀 Usage

### Physical Mode (Minimal Movement)
- Check "Enable jiggle?" only
- Cursor moves 1 pixel every second (barely visible)
- SetThreadExecutionState keeps system awake

### Zen Mode (Invisible)
- Check both "Enable jiggle?" and "Zen jiggle?"
- No cursor movement at all
- Pure execution state API usage
- Completely invisible to user

For detailed usage, configuration, and building options, see the **[Mishka README](./artifacts/Mishka/README.md)**.

## 📜 Requirements

- Windows 10/11
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

---
*© 2026 iliko*

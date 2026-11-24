# Mishka 🖱️ v1.3.8

Mishka (Russian for "Little Mouse") is a robust, high-visibility mouse jiggler application for Windows 11. It prevents screen saver activation and system sleep by simulating mouse activity with advanced features like scheduled automation, global hotkeys, and system tray integration.

## ✨ Features

- 🖱️ **System Tray Integration**: Minimize to tray to keep your taskbar clean. Double-click to restore.
- ⚙️ **Advanced Settings**: Configure jiggle intervals, startup behavior, and global hotkeys.
- ⌨️ **Global Hotkeys**: Show or hide the application from anywhere using a custom key combination (Default: `Ctrl+Shift+J`).
- 🛡️ **Conflict Detection**: Automatically detects if your chosen hotkey is already in use by another application and prevents saving problematic shortcuts.
- 📅 **Scheduled Automation**: Set specific days and times for Mishka to work automatically (e.g., Mon-Fri, 09:00 - 17:00).
- 🚀 **Auto-Startup**: Option to start automatically with Windows via the registry.
- 👻 **Tray-Only Mode**: "Start Minimized" option allows the app to launch directly to the system tray without showing a window.
- 🎯 **Physical Jiggle Mode**: Slightly moves the mouse cursor back and forth to prevent screensaver activation.
- 🎨 **High-Visibility UI**: A clean, modern Fluent 2-inspired light theme with maximum contrast for better accessibility.
- 🔍 **Runtime Validation**: Automatically detects if the required .NET 8 Runtime is missing and provides a direct download link.
- 🛠️ **Robust Stability**: Thread-safe implementation with comprehensive error handling to prevent hangs and crashes.

## 🆕 What's New in v1.3.7

- **Fixed**: About window version and Copyright lines not visible — removed spacer row that pushed content off-screen, added fallback version text, fixed Copyright text contrast
- **Fixed**: All merge conflicts resolved in `MouseSimulator.cs` — combined local Zen mode cursor movement with remote `jiggleOffset` variable
- **Fixed**: Duplicate `modStr` variable in `MainWindow.xaml.cs`
- **Fixed**: Nullable dereference warnings in `SettingsWindow.xaml.cs` and `StartupManager.cs`
- **Fixed**: SDK 11 `GenerateBundle` crash by disabling single-file publishing
- **Added**: Assembly version metadata (1.3.7)

## 🆕 What's New in v1.3.8

- **Removed**: Zen jiggle mode entirely — even minimal cursor movement was considered unwanted; only Physical Mode remains
- **Removed**: Copyright line from About window for cleaner UI
- **Removed**: Zen Mode checkbox from main window UI
- **Updated**: About window layout — removed empty spacer row, version text now shows reliably

## 📋 System Requirements

- **OS**: Windows 11 (Recommended) or Windows 10.
- **Runtime**: [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0).

## 🚀 Quick Start

1. **Build the app** using the provided `build.ps1` script (see below).
2. **Run `Mishka.exe`**.
3. Use the **Gear Icon (⚙️)** to access settings and configure your hotkey and schedule.
4. Check **"Enable jiggle?"** to start preventing system sleep.

## 🛠️ Building from Source

Mishka includes a comprehensive PowerShell build script with multiple options.

### Basic Build
```powershell
.\build.ps1
```

### Build and Run Immediately
```powershell
.\build.ps1 -Run
```

### Install PowerShell Module
Mishka can also install a companion PowerShell module for CLI control:
```powershell
.\build.ps1 -InstallPSModule
```

### Clean and Build (Self-Contained)
If you want to create a portable version that includes the .NET runtime (~150 MB):
```powershell
.\build.ps1 -Clean -SelfContained
```

## 📂 Project Structure

- `MainWindow.xaml`: Main user interface.
- `SettingsWindow.xaml`: Configuration dialog with conflict detection.
- `TrayIconManager.cs`: Handles system tray icon and context menu.
- `HotkeyManager.cs`: Manages global keyboard shortcuts.
- `MouseSimulator.cs`: Core logic for physical and virtual mouse movement.
- `ScheduleManager.cs`: Logic for automated time-based jiggling.
- `SettingsManager.cs`: Persists user preferences to `%APPDATA%\Mishka\settings.json`.
- `RuntimeDetector.cs`: Environment validation for .NET 8.
- `Styles.xaml`: High-visibility Fluent 2 styling.

## 📜 Technical Details

Mishka interacts with the Windows API (`user32.dll`) to simulate input at a low level, ensuring compatibility with most system policies. It uses `RegisterHotKey` for reliable global shortcut handling and the Windows Registry for auto-startup persistence.

---
*Created by iliko - 2026*

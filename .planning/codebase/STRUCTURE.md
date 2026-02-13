# Codebase Structure

**Analysis Date:** 2026-02-12

## Directory Layout

```
/mnt/c/TFS/RSDC 4.1 - Mishka/
├── .tmp/                   # Temporary/Intermediate files (contains MouseJiggler project)
├── artifacts/
│   └── Mishka/             # Main Project Source Code
│       ├── bin/            # Build outputs
│       ├── obj/            # Intermediate build files
│       ├── *.xaml          # UI Definitions
│       ├── *.cs            # Source files (App, Windows, Managers)
│       └── *.csproj        # Project Configuration
├── RSDC 4.1 - Mishka.sln   # Solution File
└── skills/                 # Agentic skills documentation
```

## Directory Purposes

**artifacts/Mishka:**
- Purpose: Contains the entire source code for the enhanced "Mishka" application.
- Contains: C# source files, XAML UI files, Project file.
- Key files: `App.xaml.cs`, `MainWindow.xaml.cs`, `MouseSimulator.cs`.

**.tmp/MouseJiggler:**
- Purpose: Contains the "MouseJiggler" project (likely the base or previous version).
- Contains: Similar structure to Mishka but less enhanced.

## Key File Locations

**Entry Points:**
- `artifacts/Mishka/App.xaml.cs`: Application startup and lifecycle management.
- `artifacts/Mishka/App.xaml`: XAML entry definition.

**Configuration:**
- `artifacts/Mishka/SettingsManager.cs`: Handles loading and saving settings.
- `artifacts/Mishka/Mishka.csproj`: Project build configuration and dependencies.

**Core Logic:**
- `artifacts/Mishka/MouseSimulator.cs`: The core "jiggle" functionality.
- `artifacts/Mishka/MainWindow.xaml.cs`: Main application logic and UI orchestration.

**Managers (Service Layer):**
- `artifacts/Mishka/HotkeyManager.cs`: Global hotkey handling.
- `artifacts/Mishka/ScheduleManager.cs`: Task scheduling.
- `artifacts/Mishka/TrayIconManager.cs`: System tray integration.
- `artifacts/Mishka/RuntimeDetector.cs`: Environment validation.

## Naming Conventions

**Files:**
- PascalCase: `MainWindow.xaml.cs`, `MouseSimulator.cs`
- Suffixes: `Manager` for services (e.g., `HotkeyManager`), `Window` for UI (e.g., `SettingsWindow`).

**Classes:**
- PascalCase matching filename.

## Where to Add New Code

**New Feature:**
- Logic: Create a new `*Manager.cs` class in `artifacts/Mishka/`.
- UI: Add controls to `MainWindow.xaml` or create a new `*Window.xaml`.
- Integration: Instantiate and subscribe to events in `MainWindow.xaml.cs`.

**New Setting:**
- Definition: Add property to `JiggleSettings` class (inside `SettingsManager.cs` or separate file if extracted).
- UI: Add control to `SettingsWindow.xaml` and bind/handle in `SettingsWindow.xaml.cs`.

**New Native Capability:**
- Implementation: Add P/Invoke definitions to `MouseSimulator.cs` or create a new `NativeMethods` helper if complex.

## Special Directories

**bin/ & obj/:**
- Purpose: Build artifacts.
- Generated: Yes.
- Committed: No (typically).

---

*Structure analysis: 2026-02-12*

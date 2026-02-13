# Architecture

**Analysis Date:** 2026-02-12

## Pattern Overview

**Overall:** WPF Desktop Application with Event-Driven Architecture

**Key Characteristics:**
- **Hybrid WPF/WinForms:** Uses WPF for main UI and WinForms for System Tray integration.
- **Manager Pattern:** Encapsulates system features (Hotkeys, Schedule, Tray) into dedicated manager classes.
- **Static Core:** Core functionality (`MouseSimulator`) is implemented as a static service using P/Invoke.
- **Defensive Startup:** Validates runtime environment and feature support (Tray Icon) during startup in `App.xaml.cs`.

## Layers

**Presentation (UI):**
- Purpose: User interaction and visual feedback.
- Location: `artifacts/Mishka/*.xaml`
- Contains: XAML definitions and code-behind.
- Key Files:
  - `artifacts/Mishka/MainWindow.xaml`: Main dashboard.
  - `artifacts/Mishka/SettingsWindow.xaml`: Configuration dialog.
  - `artifacts/Mishka/AboutWindow.xaml`: Information dialog.

**Application Orchestration:**
- Purpose: Application lifecycle and component wiring.
- Location: `artifacts/Mishka/App.xaml.cs`, `artifacts/Mishka/MainWindow.xaml.cs`
- Responsibilities:
  - `App.xaml.cs`: Runtime validation, global exception handling, tray support check.
  - `MainWindow.xaml.cs`: Instantiates managers, handles events, updates UI state.

**Service Layer (Managers):**
- Purpose: Encapsulate specific system integrations and business logic.
- Location: `artifacts/Mishka/*Manager.cs`
- Components:
  - `TrayIconManager`: System tray integration.
  - `HotkeyManager`: Global hotkey handling.
  - `ScheduleManager`: Time-based automation.
  - `SettingsManager`: Persistence of user preferences.
  - `StartupManager`: Windows startup registry management.

**Core Logic:**
- Purpose: The actual mouse movement simulation.
- Location: `artifacts/Mishka/MouseSimulator.cs`
- Pattern: Static service with P/Invoke.
- Depends on: `user32.dll` (Windows API).

## Data Flow

**Jiggle Activation:**

1. **Trigger:** User clicks checkbox OR Hotkey pressed OR Schedule triggers.
2. **Orchestrator:** `MainWindow.xaml.cs` receives event/input.
3. **State Update:** Updates UI (Zen mode check, Status text).
4. **Action:** Calls `MouseSimulator.Start()`.
5. **Persistence:** Saves state via `SettingsManager.Save()`.

**Startup Flow:**

1. `App.xaml.cs` triggers `OnStartup`.
2. `RuntimeDetector.ValidateRuntime()` checks for .NET environment.
3. `ValidateTrayIconSupport()` checks if NotifyIcon is supported.
4. `MainWindow` is initialized.
5. `SettingsManager` loads configuration.
6. Managers (`Hotkey`, `Schedule`, `Tray`) are initialized and wired up.

## Key Abstractions

**MouseSimulator:**
- Purpose: Low-level mouse input simulation.
- Examples: `artifacts/Mishka/MouseSimulator.cs`
- Pattern: Static Service / P/Invoke Wrapper.

**Manager Classes:**
- Purpose: Modularize distinct system features.
- Examples: `artifacts/Mishka/HotkeyManager.cs`, `artifacts/Mishka/ScheduleManager.cs`
- Pattern: Event Publisher.

## Entry Points

**Application Entry:**
- Location: `artifacts/Mishka/App.xaml.cs`
- Triggers: Application launch.
- Responsibilities: Runtime checks, global exception handling, shutting down if validation fails.

**Main Window:**
- Location: `artifacts/Mishka/MainWindow.xaml.cs`
- Triggers: Instantiated by `App.xaml` (via StartupUri or explicit creation).
- Responsibilities: Composition root for UI logic.

## Error Handling

**Strategy:** Global and Local Try-Catch blocks.

**Patterns:**
- **Startup Safety:** `App.xaml.cs` wraps startup logic to catch crash-causing errors before UI shows.
- **Graceful Degradation:** If Tray Icon fails, app runs in window-only mode (`ShowFallbackMessage`).
- **User Feedback:** Uses `MessageBox` to show errors to users when UI is available.
- **Debug Logging:** Writes to `System.Diagnostics.Debug` for developer tracing.

## Cross-Cutting Concerns

**Logging:**
- Approach: `System.Diagnostics.Debug.WriteLine` (Development/Debug only). No persistent file logging observed.

**Configuration:**
- Approach: JSON serialization via `SettingsManager`.
- File: `SettingsManager.cs`

**Native Interop:**
- Approach: P/Invoke (`DllImport`) in `MouseSimulator.cs` for `SetCursorPos` and `mouse_event`.

---

*Architecture analysis: 2026-02-12*

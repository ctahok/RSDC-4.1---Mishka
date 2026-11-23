# Decision Journal - Mishka Project

## Entry 1: Phase 1 - Skill Synthesis Complete
**Date:** 2026-02-09

### Skills Created
Following RSDC v4.1 "Skill First Mandate", created 4 new skills before application logic:

#### 1. hotkey-conflict-detector
**Purpose:** Detect if Windows hotkey is already registered by another application before attempting to use it.

**Implementation Strategy:**
- Uses Windows API `RegisterHotKey` with temporary test handle
- If registration fails, hotkey is already in use
- Immediately unregisters test registration
- Returns conflict status and message to user

**Files Created:**
- skills/hotkey-conflict-detector/README.md
- skills/hotkey-conflict-detector/scripts/generate_detector.py

#### 2. system-tray-manager
**Purpose:** Manage Windows system tray icon with context menu, always visible even when window is hidden.

**Implementation Strategy:**
- Uses `System.Windows.Forms.NotifyIcon`
- Unicode emoji icon support (🖱️)
- Context menu: Show, Settings, Exit
- Double-click to show window
- Balloon tip notifications

**Files Created:**
- skills/system-tray-manager/README.md
- skills/system-tray-manager/scripts/generate_tray_manager.py

#### 3. startup-manager
**Purpose:** Manage Windows auto-startup via registry entries.

**Implementation Strategy:**
- Registry key: HKCU\Software\Microsoft\Windows\CurrentVersion\Run
- Methods: EnableStartup(), DisableStartup(), IsStartupEnabled()
- Handles single-file publish executable path correctly
- Requires user consent before enabling

**Files Created:**
- skills/startup-manager/README.md
- skills/startup-manager/scripts/generate_startup_manager.py

#### 4. ui-enhancement-wpf
**Purpose:** Enhanced WPF UI components including settings dialogs, gear icons, improved text visibility.

**Implementation Strategy:**
- Unicode gear icon (⚙️) with hover effects
- High contrast text (white #FFFFFFFF on dark)
- Settings dialog with scrollable content
- About dialog with GitHub hyperlink
- Reusable styles in Styles.xaml

**Files Created:**
- skills/ui-enhancement-wpf/README.md
- skills/ui-enhancement-wpf/scripts/generate_settings_dialog.py
- skills/ui-enhancement-wpf/scripts/generate_about_dialog.py
- skills/ui-enhancement-wpf/scripts/improve_styles.py
- skills/ui-enhancement-wpf/scripts/generate_gear_button.py

### Rationale
Following RSDC v4.1 protocol, skills must be created BEFORE application logic. This ensures:
1. Reusable components for future projects
2. Clear separation of concerns
3. Testable, modular architecture
4. Documentation for maintenance

## Entry 2: User Preferences Confirmed
**Date:** 2026-02-09

1. **Mouse Icon Source:** Unicode emoji 🖱️ (simplest, no dependencies)
2. **Hidden Startup Behavior:** Start minimized to system tray (no window shown)
3. **Hotkey Settings:** Require modifier+key, allow Win key as modifier
4. **Copyright Year:** "©Copyright "iliko" 2026"
5. **Settings Persistence:** Keep JSON in %APPDATA%\Mishka\settings.json

## Entry 3: Application Architecture
**Date:** 2026-02-09

**App Name:** Mishka  
**Executable:** mishka.exe  
**Namespace:** Mishka

**Key Features:**
- System tray icon always visible with 🖱️ emoji
- Main window simplified: just Enable/Zen checkboxes + status
- Settings moved to modal dialog (⚙️ gear button)
- About dialog with GitHub link
- Copyright footer
- High contrast white text on dark background

**File Structure:**
```
artifacts/Mishka/
├── Mishka.csproj              # Renamed from MouseJiggler
├── App.xaml + App.xaml.cs     # Updated namespace
├── MainWindow.xaml + .cs      # Simplified UI
├── SettingsWindow.xaml + .cs  # Full settings dialog
├── AboutWindow.xaml + .cs     # About dialog
├── TrayIconManager.cs         # System tray
├── HotkeyManager.cs           # Enhanced with conflict detection
├── HotkeyConflictDetector.cs  # Conflict checking
├── StartupManager.cs          # Auto-startup
├── MouseSimulator.cs          # Existing
├── ScheduleManager.cs         # existing
└── SettingsManager.cs         # Extended
```

## Entry 4: Bug Fix - Screen Lock Prevention
**Date:** 2026-02-17

### Problem Identified
User reported that Mishka.exe does not prevent screen locking. Analysis revealed:

**Root Cause:**
1. Mouse movement was only 1 pixel and immediately reversed
2. Movement happened so fast (10ms) it was invisible to users
3. Windows treats "net-zero" movement (move +1px then -1px) as no activity
4. Modern Windows 11 power management ignores synthetic micro-movements

**Why It Failed:**
```csharp
// OLD CODE - Ineffective
GetCursorPos(out POINT point);
SetCursorPos(point.X + 1, point.Y);  // Move 1px right
Thread.Sleep(10);                     // Too fast
SetCursorPos(point.X, point.Y);      // Move back immediately
```

This creates a "blink" that:
- Is invisible to human eyes
- Appears as zero net movement to Windows
- Does not reset the idle timer reliably

### Solution Implemented

**1. Added SetThreadExecutionState API**
This is the **official Microsoft-recommended way** to prevent sleep/screensaver:

```csharp
[DllImport("kernel32.dll")]
static extern uint SetThreadExecutionState(uint esFlags);

const uint ES_CONTINUOUS = 0x80000000;
const uint ES_DISPLAY_REQUIRED = 0x00000002;  // Keep display on
const uint ES_SYSTEM_REQUIRED = 0x00000001;   // Keep system awake
```

**2. Enhanced Physical Mode (Visible Jiggle)**
```csharp
// NEW CODE - Visible and effective
jiggleOffset = (jiggleOffset == 0) ? 5 : 0;  // Alternate 0px and 5px
SetCursorPos(point.X + jiggleOffset, point.Y);
Thread.Sleep(50);  // Long enough to see
SetCursorPos(point.X, point.Y);  // Return to original
```

Benefits:
- 5px movement is visible (user can confirm it's working)
- 50ms delay makes movement perceptible
- Alternating pattern creates clear "jiggle" effect

**3. Refined Zen Mode**
```csharp
// Zen mode: Execution state only (no cursor movement)
SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
```

Benefits:
- No visible cursor movement (true "zen")
- Uses official Windows API
- Works even with strict corporate security policies
- Refreshed every interval to maintain awake state

### Technical Details

**Start() Method:**
- Sets execution state immediately when jiggle starts
- Flags: `ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED`
- Effect: Tells Windows "application is busy, don't sleep or turn off display"

**Stop() Method:**
- Restores normal power management
- Calls `SetThreadExecutionState(ES_CONTINUOUS)` to clear flags
- Allows system to sleep/screensaver normally when stopped

**Physical Mode Behavior:**
- Every interval (default 1000ms): Move cursor 5px right → wait 50ms → move back
- Visual confirmation that app is working
- Execution state keeps system awake between movements

**Zen Mode Behavior:**
- No cursor movement at all
- Purely relies on execution state API
- Refreshed every interval to maintain state

### Why This Solution is Superior

1. **Industry Standard**: `SetThreadExecutionState` is the official Microsoft API for this purpose
2. **Reliable**: Works across all Windows power management configurations
3. **Visible Feedback**: Physical mode shows clear cursor jiggle
4. **Corporate-Friendly**: Zen mode works even with mouse input monitoring
5. **Power Aware**: Properly restores power management when stopped

### Files Modified
- `artifacts/Mishka/MouseSimulator.cs`:
  - Added `SetThreadExecutionState` P/Invoke (line 18-19)
  - Added execution state constants (lines 30-33)
  - Added `jiggleOffset` tracking variable (line 39)
  - Modified `Start()` to set execution state (line 53)
  - Modified `Stop()` to restore power management (line 68)
  - Completely rewrote `JiggleLoop()` (lines 77-111)

### Expected Behavior After Fix

**Physical Mode:**
- User sees cursor "jiggle" 5 pixels right/left every second
- Screen does not lock
- Screensaver does not activate
- System does not sleep

**Zen Mode:**
- Cursor remains perfectly still
- Screen does not lock
- Screensaver does not activate
- System does not sleep
- No visible indication (pure "zen")

### Namespace Ambiguity Fixes

Also resolved two build errors:

1. **App.xaml.cs (line 5)**: `Application` → `System.Windows.Application`
   - Disambiguates between WPF and WinForms Application classes

2. **ScheduleManager.cs (line 10)**: `Timer` → `System.Threading.Timer`
   - Disambiguates between WinForms and Threading Timer classes

These were caused by the project using both `UseWPF` and `UseWindowsForms` (needed for NotifyIcon).

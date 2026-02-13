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
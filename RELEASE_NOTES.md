# Mishka Release Notes

## Version 1.1.1 (2026-02-17)

### 🎯 What's New
- **Minimal Mouse Movement**: Reduced physical mode movement from 5 pixels to 1 pixel (barely noticeable)
- **Dynamic Version Display**: About window now shows version automatically from assembly metadata
- **Faster Response**: Movement delay reduced to 10ms for near-instant return to original position

### 🐛 Bug Fixes
- Fixed screen lock prevention using official Windows `SetThreadExecutionState` API
- Fixed single-file publish compatibility with `Environment.ProcessPath` (replaced `Assembly.Location`)
- Fixed all C# nullable reference type warnings (CS8618, CS8600, CS8602)
- Fixed IL3000 warning for single-file app compatibility
- Fixed namespace ambiguity errors (CS0104) in MessageBox and Timer references

### 🔧 Technical Improvements
- **Build Quality**: Clean build with 0 errors and 0 warnings
- **Modern C# Support**: Full .NET 8 nullable reference types compliance
- **Reliable Prevention**: Uses `ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED` flags
- **Executable Size**: 286 KB (framework-dependent)

### 📦 Distribution
- **Framework-Dependent**: Requires .NET 8 Runtime on target machine
- **Single-File**: Published as single executable
- **Platform**: Windows 10/11 (x64)

### 🎮 Usage
- **Physical Mode** (default): 1-pixel cursor jiggle every second
  - Movement is barely visible but prevents screen lock
  - SetThreadExecutionState keeps system awake
  
- **Zen Mode** (enable both checkboxes): No cursor movement at all
  - Pure execution state API usage
  - Completely invisible to user
  - Still prevents screen lock and sleep

### 🔄 Migration from 1.1.0
- Simply replace the executable
- No settings changes required
- Movement is now much more subtle

---

## Version 1.1.0 (2026-02-13)

### Initial Features
- Mouse jiggling to prevent screen saver
- System tray integration with 🖱️ icon
- Global hotkey support (Ctrl+Shift+J)
- Schedule automation (Mon-Fri 9-5)
- Settings persistence
- Auto-startup with Windows
- Dark theme UI with high contrast

---

## Installation

### Requirements
- Windows 10/11 (64-bit)
- .NET 8 Runtime ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))

### Quick Start
1. Download `Mishka.exe` from releases
2. Run the executable
3. Check "Enable jiggle?" to start
4. Optionally enable "Zen jiggle?" for invisible mode

### Building from Source
```bash
cd artifacts/Mishka
dotnet publish -c Release -o ../output
```

Output: `output/Mishka.exe`

---

**Copyright © "iliko" 2026**  
**License**: MIT  
**GitHub**: https://github.com/ctahok/RSDC-4.1---Mishka

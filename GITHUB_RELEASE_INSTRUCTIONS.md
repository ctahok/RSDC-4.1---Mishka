# GitHub Release Instructions for v1.1.1

## ✅ Completed Steps

1. ✅ Version bumped to 1.1.1 in Mishka.csproj
2. ✅ Mouse movement reduced to 1 pixel (barely noticeable)
3. ✅ Dynamic version display added to About window
4. ✅ All code committed (6 commits)
5. ✅ Git tag `v1.1.1` created with release notes
6. ✅ Branch `1.1` pushed to GitHub
7. ✅ Tag `v1.1.1` pushed to GitHub
8. ✅ Release executable ready at: `releases/v1.1.1/Mishka.exe` (286 KB)
9. ✅ Documentation updated (README.md, RELEASE_NOTES.md)

## 📦 Create GitHub Release (Manual Steps)

Since GitHub CLI is not available, please create the release manually:

### Step 1: Navigate to Releases
Go to: https://github.com/ctahok/RSDC-4.1---Mishka/releases/new

### Step 2: Configure Release
- **Choose a tag**: Select `v1.1.1` from dropdown
- **Release title**: `Mishka v1.1.1 - Minimal Movement & Dynamic Versioning`
- **Target**: `1.1` branch

### Step 3: Upload Executable
Click "Attach binaries" and upload:
- File: `C:\TFS\RSDC 4.1 - Mishka\releases\v1.1.1\Mishka.exe`
- Size: 286 KB

### Step 4: Add Release Notes

Copy and paste the following:

```markdown
## 🎯 What's New

- **Minimal Mouse Movement**: Reduced physical mode movement from 5 pixels to 1 pixel (barely noticeable)
- **Dynamic Version Display**: About window now shows version automatically from assembly metadata
- **Faster Response**: Movement delay reduced to 10ms for near-instant return to original position

## 🐛 Bug Fixes

- Fixed screen lock prevention using official Windows `SetThreadExecutionState` API
- Fixed single-file publish compatibility with `Environment.ProcessPath`
- Fixed all C# nullable reference type warnings (CS8618, CS8600, CS8602)
- Fixed IL3000 warning for single-file app compatibility
- Fixed namespace ambiguity errors in MessageBox and Timer references

## 🔧 Technical Improvements

- **Build Quality**: Clean build with 0 errors and 0 warnings
- **Modern C# Support**: Full .NET 8 nullable reference types compliance
- **Reliable Prevention**: Uses `ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED` flags
- **Executable Size**: 286 KB (framework-dependent)

## 📦 Installation

### Requirements
- Windows 10/11 (64-bit)
- [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

### Quick Start
1. Download `Mishka.exe` below
2. Install .NET 8 Runtime if not already installed
3. Run the executable
4. Check "Enable jiggle?" to start

## 🎮 Usage

### Physical Mode (Minimal Movement)
- Check "Enable jiggle?" only
- Cursor moves 1 pixel every second (barely visible)
- SetThreadExecutionState keeps system awake

### Zen Mode (Invisible)
- Check both "Enable jiggle?" and "Zen jiggle?"
- No cursor movement at all
- Pure execution state API usage
- Completely invisible to user

## 📝 Full Release Notes

See [RELEASE_NOTES.md](https://github.com/ctahok/RSDC-4.1---Mishka/blob/1.1/RELEASE_NOTES.md) for complete details.

---

**Copyright © "iliko" 2026** | [GitHub Repository](https://github.com/ctahok/RSDC-4.1---Mishka)
```

### Step 5: Publish
- Check "Set as the latest release"
- Click "Publish release"

## 🎉 Done!

After publishing, the release will be available at:
https://github.com/ctahok/RSDC-4.1---Mishka/releases/tag/v1.1.1

---

## 📊 Release Summary

| Item | Value |
|------|-------|
| Version | 1.1.1 |
| Branch | 1.1 |
| Executable | Mishka.exe (286 KB) |
| Build Warnings | 0 |
| Build Errors | 0 |
| Mouse Movement | 1 pixel (minimal) |
| API | SetThreadExecutionState |
| Target Framework | .NET 8.0 Windows |
| Platform | win-x64 |

## 🔗 Important Links

- Repository: https://github.com/ctahok/RSDC-4.1---Mishka
- Release Notes: [RELEASE_NOTES.md](../RELEASE_NOTES.md)
- Main README: [README.md](../README.md)
- Tag v1.1.1: https://github.com/ctahok/RSDC-4.1---Mishka/releases/tag/v1.1.1

---
name: hotkey-conflict-detector
description: Detect Windows hotkey conflicts before registration using Windows API test registration
version: 1.0
---

# Hotkey Conflict Detector

## Usage
Use this skill when implementing global hotkeys in Windows applications to detect if a keyboard shortcut is already registered by another application before attempting to use it.

## Instructions
1. Run `generate_detector.py` to create the conflict detection class
2. Call `CheckConflict()` before registering a hotkey
3. Display warning to user if conflict detected

## Tools (Scripts)
*   **Generate Detector:** `python scripts/generate_detector.py --output <path>`

## Dependencies
*   Python 3.8+
*   Windows OS (uses user32.dll RegisterHotKey)

## Implementation Strategy
Uses Windows API test registration:
1. Attempt to register hotkey with temporary ID
2. If RegisterHotKey returns FALSE, hotkey is already in use
3. Immediately unregister the test registration
4. Return conflict status to caller
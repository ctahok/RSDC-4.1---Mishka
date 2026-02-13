---
name: ui-enhancement-wpf
description: Enhanced WPF UI components including settings dialogs, gear icons, improved text visibility, and about dialogs
version: 1.0
---

# UI Enhancement WPF

## Usage
Use this skill when improving WPF application UI with better visibility, icons, and dialogs. Generates settings dialogs, gear buttons, and about dialogs with proper styling.

## Instructions
1. Run `generate_settings_dialog.py` to create SettingsWindow
2. Run `generate_about_dialog.py` to create AboutWindow
3. Run `improve_styles.py` to enhance text visibility
4. Run `generate_gear_button.py` to create gear icon button

## Tools (Scripts)
*   **Settings Dialog:** `python scripts/generate_settings_dialog.py --output <path>`
*   **About Dialog:** `python scripts/generate_about_dialog.py --output <path> --url <github-url>`
*   **Improve Styles:** `python scripts/improve_styles.py --output <path>`
*   **Gear Button:** `python scripts/generate_gear_button.py --output <path>`

## Dependencies
*   Python 3.8+
*   WPF / .NET

## Features
*   ⚙️ Unicode gear icon button
*   High contrast text (white on dark)
*   Modal settings dialog
*   About dialog with hyperlink
*   Consistent dark theme styling
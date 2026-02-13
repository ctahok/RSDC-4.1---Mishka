---
name: system-tray-manager
description: Manage Windows system tray icon with context menu, always visible even when window hidden
version: 1.0
---

# System Tray Manager

## Usage
Use this skill when creating Windows applications that need to run in the background with a system tray icon. Supports context menu, double-click to show, and Unicode emoji icons.

## Instructions
1. Run `generate_tray_manager.py` to create the tray manager class
2. Initialize in App.xaml.cs or MainWindow
3. Call Show() to display icon, Hide() to remove

## Tools (Scripts)
*   **Generate Tray Manager:** `python scripts/generate_tray_manager.py --output <path> [--icon <emoji>]`

## Dependencies
*   Python 3.8+
*   Windows Forms (System.Windows.Forms.NotifyIcon)
*   .NET 6+ or .NET Framework

## Features
*   Unicode emoji icon support (🖱️)
*   Context menu with custom items
*   Double-click to show window
*   Balloon tip notifications
*   Always visible in tray area
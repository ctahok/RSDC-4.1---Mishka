---
name: mouse-input-simulator
description: Simulate mouse movement and input via Windows API
version: 1.0
---

# Mouse Input Simulator

## Usage
Use this skill when implementing mouse jiggling, virtual mouse movement, or input simulation. Generates C# or PowerShell code using Windows API.

## Instructions
1. Run `generate_code.py` with target language
2. Copy generated code into your application
3. Call Start/Stop methods to control simulation

## Tools (Scripts)
*   **Generate Code:** `python scripts/generate_code.py --language <csharp|powershell> --mode <physical|virtual|both> --output <file>`

## Dependencies
*   Python 3.8+
*   Windows OS (uses user32.dll)
*   Target language: C# or PowerShell
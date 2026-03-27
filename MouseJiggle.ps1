param(
    [ValidateRange(1, 2147483647)]
    [int]$MinIntervalMs = 800,

    [ValidateRange(1, 2147483647)]
    [int]$MaxIntervalMs = 1200,

    [ValidateRange(1, 3600)]
    [int]$DurationMinutes = 0,

    [switch]$ZenMode
)

if ($MinIntervalMs -gt $MaxIntervalMs) {
    throw "MinIntervalMs must be less than or equal to MaxIntervalMs."
}

Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;

public static class MouseJiggleNative
{
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    [DllImport("kernel32.dll")]
    public static extern uint SetThreadExecutionState(uint esFlags);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }
}
"@

$script:ES_CONTINUOUS = 0x80000000
$script:ES_SYSTEM_REQUIRED = 0x00000001
$script:ES_DISPLAY_REQUIRED = 0x00000002

function Start-ExecutionStateKeepAlive {
    [MouseJiggleNative]::SetThreadExecutionState(
        $script:ES_CONTINUOUS -bor $script:ES_DISPLAY_REQUIRED -bor $script:ES_SYSTEM_REQUIRED
    ) | Out-Null
}

function Stop-ExecutionStateKeepAlive {
    [MouseJiggleNative]::SetThreadExecutionState($script:ES_CONTINUOUS) | Out-Null
}

function Get-RandomIntervalMs {
    if ($MinIntervalMs -eq $MaxIntervalMs) {
        return $MinIntervalMs
    }

    Get-Random -Minimum $MinIntervalMs -Maximum ($MaxIntervalMs + 1)
}

function Invoke-PhysicalJiggle {
    $point = New-Object MouseJiggleNative+POINT
    [MouseJiggleNative]::GetCursorPos([ref]$point) | Out-Null

    [MouseJiggleNative]::SetCursorPos($point.X + 1, $point.Y) | Out-Null
    Start-Sleep -Milliseconds 10
    [MouseJiggleNative]::SetCursorPos($point.X, $point.Y) | Out-Null
}

function Invoke-ZenRefresh {
    Start-ExecutionStateKeepAlive
}

try {
    Start-ExecutionStateKeepAlive

    $endTime = if ($DurationMinutes -gt 0) {
        (Get-Date).AddMinutes($DurationMinutes)
    } else {
        $null
    }

    Write-Host "Mouse jiggle started. Press Ctrl+C to stop."
    Write-Host ("Mode: {0}" -f ($(if ($ZenMode) { 'Zen' } else { 'Physical' })))
    Write-Host ("Random interval: {0}ms - {1}ms" -f $MinIntervalMs, $MaxIntervalMs)

    while ($true) {
        if ($endTime -and (Get-Date) -ge $endTime) {
            break
        }

        if ($ZenMode) {
            Invoke-ZenRefresh
        } else {
            Invoke-PhysicalJiggle
        }

        Start-Sleep -Milliseconds (Get-RandomIntervalMs)
    }
}
finally {
    Stop-ExecutionStateKeepAlive
    Write-Host "Mouse jiggle stopped."
}

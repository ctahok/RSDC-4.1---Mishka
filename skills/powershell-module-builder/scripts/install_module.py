#!/usr/bin/env python3
"""
Install PowerShell module to user's module path.
"""
import argparse
import os
import shutil
import subprocess
import sys
from pathlib import Path

def get_ps_module_path(scope: str = 'CurrentUser') -> str:
    """Get PowerShell modules directory path."""
    try:
        result = subprocess.run(
            ['powershell', '-Command', f'Write-Output $env:PSModulePath'],
            capture_output=True, text=True
        )
        if result.returncode != 0:
            raise RuntimeError("Could not get PSModulePath")
        
        paths = result.stdout.strip().split(';')
        # Filter based on scope
        for path in paths:
            if scope == 'CurrentUser' and ('Documents' in path or 'User' in path):
                return path
            elif scope == 'AllUsers' and 'Program Files' in path:
                return path
        
        # Fallback to first path
        return paths[0] if paths else os.path.expanduser('~/Documents/WindowsPowerShell/Modules')
    except Exception as e:
        print(f"Warning: Could not detect PSModulePath ({e}), using default")
        return os.path.expanduser('~/Documents/PowerShell/Modules')

def install_module(module_dir: str, scope: str = 'CurrentUser') -> bool:
    """Install module to PowerShell modules directory."""
    try:
        module_name = os.path.basename(module_dir)
        module_path = get_ps_module_path(scope)
        dest_dir = os.path.join(module_path, module_name)
        
        print(f"Installing {module_name} to {dest_dir}")
        
        # Remove existing
        if os.path.exists(dest_dir):
            shutil.rmtree(dest_dir)
        
        # Copy module
        shutil.copytree(module_dir, dest_dir)
        
        print(f"✓ Module installed: {dest_dir}")
        print(f"\nImport with: Import-Module {module_name}")
        return True
        
    except Exception as e:
        print(f"ERROR: Installation failed - {e}", file=sys.stderr)
        return False

def main():
    parser = argparse.ArgumentParser(description='Install PowerShell module')
    parser.add_argument('--path', required=True, help='Module directory path')
    parser.add_argument('--scope', default='CurrentUser', choices=['CurrentUser', 'AllUsers'])
    args = parser.parse_args()
    
    if not os.path.exists(args.path):
        print(f"ERROR: Module directory not found: {args.path}", file=sys.stderr)
        sys.exit(1)
    
    success = install_module(args.path, args.scope)
    sys.exit(0 if success else 1)

if __name__ == '__main__':
    main()
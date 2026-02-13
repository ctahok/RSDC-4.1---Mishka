#!/usr/bin/env python3
"""
Build self-contained WPF executable.
"""
import argparse
import os
import subprocess
import sys
from pathlib import Path

def find_csproj(project_dir: str) -> str:
    """Find .csproj file in project directory."""
    csproj_files = list(Path(project_dir).glob('*.csproj'))
    if not csproj_files:
        raise FileNotFoundError(f"No .csproj file found in {project_dir}")
    return str(csproj_files[0])

def build_executable(project_dir: str, output_dir: str) -> bool:
    """Build self-contained executable using dotnet publish."""
    try:
        csproj = find_csproj(project_dir)
        print(f"Building: {csproj}")
        
        cmd = [
            'dotnet', 'publish', csproj,
            '-c', 'Release',
            '-o', output_dir,
            '--self-contained', 'true',
            '-r', 'win-x64',
            '/p:PublishSingleFile=true'
        ]
        
        result = subprocess.run(cmd, capture_output=True, text=True)
        
        if result.returncode != 0:
            print(f"ERROR: Build failed\n{result.stderr}", file=sys.stderr)
            return False
        
        exe_files = list(Path(output_dir).glob('*.exe'))
        if exe_files:
            print(f"✓ Executable created: {exe_files[0]}")
        else:
            print("⚠ Warning: No .exe file found in output")
        
        return True
        
    except FileNotFoundError as e:
        print(f"ERROR: {e}", file=sys.stderr)
        return False
    except Exception as e:
        print(f"ERROR: Build failed - {e}", file=sys.stderr)
        return False

def main():
    parser = argparse.ArgumentParser(description='Build WPF executable')
    parser.add_argument('--project', required=True, help='Project directory path')
    parser.add_argument('--output', required=True, help='Output directory for .exe')
    args = parser.parse_args()
    
    if not os.path.exists(args.project):
        print(f"ERROR: Project directory not found: {args.project}", file=sys.stderr)
        sys.exit(1)
    
    os.makedirs(args.output, exist_ok=True)
    
    success = build_executable(args.project, args.output)
    sys.exit(0 if success else 1)

if __name__ == '__main__':
    main()
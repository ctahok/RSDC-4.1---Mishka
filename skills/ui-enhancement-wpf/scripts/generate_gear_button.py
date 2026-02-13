#!/usr/bin/env python3
"""
Generate gear button XAML snippet.
"""
import argparse

GEAR_BUTTON = '''<Button x:Name="btnSettings"
        Style="{StaticResource GearButtonStyle}"
        ToolTip="Settings"
        Click="BtnSettings_Click"/>'''

def main():
    parser = argparse.ArgumentParser(description='Generate gear button XAML')
    parser.add_argument('--output', required=True, help='Output file path')
    args = parser.parse_args()
    
    with open(args.output, 'w') as f:
        f.write(GEAR_BUTTON)
    
    print(f"✓ Generated gear button snippet: {args.output}")
    print("Copy this XAML into your MainWindow.xaml header")

if __name__ == '__main__':
    main()
#!/usr/bin/env python3
"""
Create WPF project scaffold with modern minimalistic design.
"""
import argparse
import os
import sys
from pathlib import Path

def create_csproj(project_dir: str, project_name: str) -> str:
    """Generate .csproj file for self-contained executable."""
    content = f'''<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UseWPF>true</UseWPF>
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>true</SelfContained>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <AssemblyTitle>{project_name}</AssemblyTitle>
  </PropertyGroup>

</Project>'''
    path = os.path.join(project_dir, f"{project_name}.csproj")
    with open(path, 'w') as f:
        f.write(content)
    return path

def create_app_xaml(project_dir: str, project_name: str) -> str:
    """Generate App.xaml with dark theme resource dictionary."""
    content = f'''<Application x:Class="{project_name}.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Styles.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>'''
    path = os.path.join(project_dir, "App.xaml")
    with open(path, 'w') as f:
        f.write(content)
    return path

def create_app_xaml_cs(project_dir: str, project_name: str) -> str:
    """Generate App.xaml.cs code-behind."""
    content = f'''using System.Windows;

namespace {project_name}
{{
    public partial class App : Application
    {{
        protected override void OnStartup(StartupEventArgs e)
        {{
            base.OnStartup(e);
        }}
    }}
}}'''
    path = os.path.join(project_dir, "App.xaml.cs")
    with open(path, 'w') as f:
        f.write(content)
    return path

def create_styles_xaml(project_dir: str) -> str:
    """Generate minimalistic dark theme styles."""
    content = '''<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <SolidColorBrush x:Key="BackgroundBrush" Color="#FF1E1E1E"/>
    <SolidColorBrush x:Key="ForegroundBrush" Color="#FFE0E0E0"/>
    <SolidColorBrush x:Key="BorderBrush" Color="#FF3E3E3E"/>
    <SolidColorBrush x:Key="AccentBrush" Color="#FF007ACC"/>
    <SolidColorBrush x:Key="HoverBrush" Color="#FF2E2E2E"/>
    
    <Style TargetType="Window">
        <Setter Property="Background" Value="{StaticResource BackgroundBrush}"/>
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="FontFamily" Value="Segoe UI"/>
        <Setter Property="FontSize" Value="12"/>
    </Style>
    
    <Style TargetType="CheckBox">
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="Margin" Value="5"/>
        <Setter Property="VerticalContentAlignment" Value="Center"/>
    </Style>
    
    <Style TargetType="Button">
        <Setter Property="Background" Value="{StaticResource AccentBrush}"/>
        <Setter Property="Foreground" Value="White"/>
        <Setter Property="Padding" Value="15,5"/>
        <Setter Property="Margin" Value="5"/>
        <Setter Property="BorderThickness" Value="0"/>
        <Setter Property="Cursor" Value="Hand"/>
        <Style.Triggers>
            <Trigger Property="IsMouseOver" Value="True">
                <Setter Property="Background" Value="#FF005999"/>
            </Trigger>
        </Style.Triggers>
    </Style>
    
    <Style TargetType="TextBox">
        <Setter Property="Background" Value="{StaticResource HoverBrush}"/>
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="Padding" Value="5"/>
        <Setter Property="Margin" Value="5"/>
    </Style>
    
    <Style TargetType="ComboBox">
        <Setter Property="Background" Value="{StaticResource HoverBrush}"/>
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="Margin" Value="5"/>
        <Setter Property="Padding" Value="5"/>
    </Style>
    
    <Style TargetType="GroupBox">
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="Margin" Value="5"/>
        <Setter Property="Padding" Value="10"/>
    </Style>
    
    <Style TargetType="Label">
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="Margin" Value="2"/>
    </Style>
    
</ResourceDictionary>'''
    path = os.path.join(project_dir, "Styles.xaml")
    with open(path, 'w') as f:
        f.write(content)
    return path

def main():
    parser = argparse.ArgumentParser(description='Create WPF project scaffold')
    parser.add_argument('--name', required=True, help='Project name')
    parser.add_argument('--output', required=True, help='Output directory')
    parser.add_argument('--template', default='minimal', choices=['minimal', 'standard'])
    args = parser.parse_args()
    
    project_dir = os.path.join(args.output, args.name)
    os.makedirs(project_dir, exist_ok=True)
    
    print(f"Creating WPF project: {args.name}")
    
    create_csproj(project_dir, args.name)
    create_app_xaml(project_dir, args.name)
    create_app_xaml_cs(project_dir, args.name)
    create_styles_xaml(project_dir)
    
    print(f"✓ Project created at: {project_dir}")
    print("  - Project file (.csproj)")
    print("  - App.xaml / App.xaml.cs")
    print("  - Styles.xaml (dark theme)")
    print("\nNext: Add MainWindow.xaml and logic files")

if __name__ == '__main__':
    main()
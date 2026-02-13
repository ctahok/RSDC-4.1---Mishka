import argparse
import os
import subprocess
import sys

def create_wpf_project(name, output_path):
    """Create a new WPF project with minimalistic design template."""
    
    project_dir = os.path.join(output_path, name)
    os.makedirs(project_dir, exist_ok=True)
    
    # Create .csproj file
    csproj_content = f'''<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UseWPF>true</UseWPF>
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>true</SelfContained>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  </PropertyGroup>

</Project>'''
    
    with open(os.path.join(project_dir, f"{name}.csproj"), 'w') as f:
        f.write(csproj_content)
    
    # Create App.xaml
    app_xaml = '''<Application x:Class="MouseJiggler.App"
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
    
    with open(os.path.join(project_dir, "App.xaml"), 'w') as f:
        f.write(app_xaml)
    
    # Create App.xaml.cs
    app_cs = '''using System.Windows;

namespace MouseJiggler
{
    public partial class App : Application
    {
    }
}'''
    
    with open(os.path.join(project_dir, "App.xaml.cs"), 'w') as f:
        f.write(app_cs)
    
    print(f"✓ Created WPF project: {project_dir}")
    return project_dir

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Create WPF project")
    parser.add_argument("--name", required=True, help="Project name")
    parser.add_argument("--output", required=True, help="Output directory")
    args = parser.parse_args()
    
    create_wpf_project(args.name, args.output)
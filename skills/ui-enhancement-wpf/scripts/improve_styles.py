#!/usr/bin/env python3
"""
Generate enhanced WPF Styles.xaml with better text visibility.
"""
import argparse

STYLES_XAML = '''<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <!-- High Contrast Colors for Better Visibility -->
    <SolidColorBrush x:Key="BackgroundBrush" Color="#FF1A1A1A"/>
    <SolidColorBrush x:Key="ForegroundBrush" Color="#FFFFFFFF"/>
    <SolidColorBrush x:Key="SecondaryTextBrush" Color="#FFCCCCCC"/>
    <SolidColorBrush x:Key="BorderBrush" Color="#FF4A4A4A"/>
    <SolidColorBrush x:Key="AccentBrush" Color="#FF007ACC"/>
    <SolidColorBrush x:Key="HoverBrush" Color="#FF2A2A2A"/>
    <SolidColorBrush x:Key="SuccessBrush" Color="#FF4CAF50"/>
    <SolidColorBrush x:Key="WarningBrush" Color="#FFFF9800"/>
    <SolidColorBrush x:Key="ErrorBrush" Color="#FFF44336"/>
    
    <!-- Window Style -->
    <Style TargetType="Window">
        <Setter Property="Background" Value="{StaticResource BackgroundBrush}"/>
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="FontFamily" Value="Segoe UI"/>
        <Setter Property="FontSize" Value="14"/>
    </Style>
    
    <!-- CheckBox Style -->
    <Style TargetType="CheckBox">
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="FontSize" Value="14"/>
        <Setter Property="Margin" Value="8"/>
        <Setter Property="VerticalContentAlignment" Value="Center"/>
    </Style>
    
    <!-- Button Style -->
    <Style TargetType="Button">
        <Setter Property="Background" Value="{StaticResource AccentBrush}"/>
        <Setter Property="Foreground" Value="White"/>
        <Setter Property="FontSize" Value="13"/>
        <Setter Property="Padding" Value="20,8"/>
        <Setter Property="Margin" Value="8"/>
        <Setter Property="BorderThickness" Value="0"/>
        <Setter Property="Cursor" Value="Hand"/>
        <Style.Triggers>
            <Trigger Property="IsMouseOver" Value="True">
                <Setter Property="Background" Value="#FF005A9E"/>
            </Trigger>
        </Style.Triggers>
    </Style>
    
    <!-- Gear Button Style (Icon Button) -->
    <Style x:Key="GearButtonStyle" TargetType="Button">
        <Setter Property="Background" Value="Transparent"/>
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="FontSize" Value="20"/>
        <Setter Property="Padding" Value="5"/>
        <Setter Property="Margin" Value="2"/>
        <Setter Property="BorderThickness" Value="0"/>
        <Setter Property="Cursor" Value="Hand"/>
        <Setter Property="Content" Value="⚙"/>
        <Style.Triggers>
            <Trigger Property="IsMouseOver" Value="True">
                <Setter Property="Foreground" Value="{StaticResource AccentBrush}"/>
            </Trigger>
        </Style.Triggers>
    </Style>
    
    <!-- TextBox Style -->
    <Style TargetType="TextBox">
        <Setter Property="Background" Value="{StaticResource HoverBrush}"/>
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="FontSize" Value="13"/>
        <Setter Property="Padding" Value="8"/>
        <Setter Property="Margin" Value="5"/>
    </Style>
    
    <!-- ComboBox Style -->
    <Style TargetType="ComboBox">
        <Setter Property="Background" Value="{StaticResource HoverBrush}"/>
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="FontSize" Value="13"/>
        <Setter Property="Margin" Value="5"/>
        <Setter Property="Padding" Value="5"/>
    </Style>
    
    <!-- GroupBox Style -->
    <Style TargetType="GroupBox">
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="BorderBrush" Value="{StaticResource BorderBrush}"/>
        <Setter Property="FontSize" Value="14"/>
        <Setter Property="Margin" Value="5"/>
        <Setter Property="Padding" Value="10"/>
    </Style>
    
    <!-- Label Style -->
    <Style TargetType="Label">
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="FontSize" Value="13"/>
        <Setter Property="Margin" Value="3"/>
    </Style>
    
    <!-- TextBlock Header Style -->
    <Style x:Key="HeaderTextStyle" TargetType="TextBlock">
        <Setter Property="Foreground" Value="{StaticResource ForegroundBrush}"/>
        <Setter Property="FontSize" Value="24"/>
        <Setter Property="FontWeight" Value="Bold"/>
        <Setter Property="Margin" Value="0,0,0,15"/>
    </Style>
    
    <!-- Status Text Style -->
    <Style x:Key="StatusTextStyle" TargetType="TextBlock">
        <Setter Property="Foreground" Value="{StaticResource SecondaryTextBrush}"/>
        <Setter Property="FontSize" Value="14"/>
        <Setter Property="FontWeight" Value="SemiBold"/>
    </Style>
    
    <!-- Copyright Text Style -->
    <Style x:Key="CopyrightTextStyle" TargetType="TextBlock">
        <Setter Property="Foreground" Value="{StaticResource SecondaryTextBrush}"/>
        <Setter Property="FontSize" Value="11"/>
        <Setter Property="HorizontalAlignment" Value="Center"/>
        <Setter Property="Margin" Value="0,10,0,0"/>
    </Style>
    
</ResourceDictionary>'''

def main():
    parser = argparse.ArgumentParser(description='Generate enhanced styles')
    parser.add_argument('--output', required=True, help='Output file path')
    args = parser.parse_args()
    
    with open(args.output, 'w') as f:
        f.write(STYLES_XAML)
    
    print(f"✓ Generated enhanced styles: {args.output}")

if __name__ == '__main__':
    main()
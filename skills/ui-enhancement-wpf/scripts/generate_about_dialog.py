#!/usr/bin/env python3
"""
Generate AboutWindow.xaml with copyright and GitHub link.
"""
import argparse

ABOUT_XAML = '''<Window x:Class="Mishka.AboutWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="About Mishka" 
        Height="350" Width="400"
        WindowStartupLocation="CenterOwner"
        ResizeMode="NoResize">
    
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- Icon and Title -->
        <StackPanel Grid.Row="0" HorizontalAlignment="Center" Margin="0,0,0,20">
            <TextBlock Text="🖱️" FontSize="48" HorizontalAlignment="Center"/>
            <TextBlock Text="Mishka" 
                       FontSize="28" 
                       FontWeight="Bold"
                       HorizontalAlignment="Center"
                       Margin="0,10,0,5"/>
            <TextBlock Text="Mouse Jiggler" 
                       FontSize="14"
                       Foreground="{StaticResource SecondaryTextBrush}"
                       HorizontalAlignment="Center"/>
        </StackPanel>
        
        <!-- Description -->
        <TextBlock Grid.Row="1" 
                   Text="Prevents screen saver activation by simulating mouse movement. Perfect for keeping your system awake during long tasks."
                   TextWrapping="Wrap"
                   TextAlignment="Center"
                   Foreground="{StaticResource SecondaryTextBrush}"
                   Margin="0,0,0,20"/>
        
        <!-- Links -->
        <StackPanel Grid.Row="2" HorizontalAlignment="Center">
            <TextBlock Margin="0,0,0,10">
                <Hyperlink x:Name="lnkGitHub" Click="LnkGitHub_Click">
                    <TextBlock Text="View on GitHub"/>
                </Hyperlink>
            </TextBlock>
            <TextBlock Text="Version 1.0.0"
                       FontSize="12"
                       Foreground="{StaticResource SecondaryTextBrush}"
                       HorizontalAlignment="Center"/>
        </StackPanel>
        
        <!-- Copyright -->
        <TextBlock Grid.Row="3" 
                   Text="©Copyright &#x22;iliko&#x22; 2026"
                   Style="{StaticResource CopyrightTextStyle}"/>
        
    </Grid>
</Window>'''

def main():
    parser = argparse.ArgumentParser(description='Generate about dialog')
    parser.add_argument('--output', required=True, help='Output file path')
    parser.add_argument('--url', default='https://github.com/ctahok/RSDC-4.1-Mishka', help='GitHub URL')
    args = parser.parse_args()
    
    with open(args.output, 'w') as f:
        f.write(ABOUT_XAML)
    
    print(f"✓ Generated about dialog: {args.output}")

if __name__ == '__main__':
    main()
#!/usr/bin/env python3
"""
Generate SettingsWindow.xaml with full settings dialog.
"""
import argparse

SETTINGS_XAML = '''<Window x:Class="Mishka.SettingsWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="⚙ Mishka Settings" 
        Height="500" Width="450"
        WindowStartupLocation="CenterOwner"
        ResizeMode="NoResize">
    
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- Header -->
        <TextBlock Grid.Row="0" 
                   Text="⚙ Settings" 
                   Style="{StaticResource HeaderTextStyle}"/>
        
        <!-- Settings Content -->
        <ScrollViewer Grid.Row="1" VerticalScrollBarVisibility="Auto">
            <StackPanel>
                
                <!-- Hotkey Section -->
                <GroupBox Header="Hotkey Configuration">
                    <StackPanel Margin="10">
                        <TextBlock Text="Toggle visibility shortcut:" 
                                   Margin="0,0,0,8"
                                   Foreground="{StaticResource SecondaryTextBrush}"/>
                        
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,10">
                            <ComboBox x:Name="cmbModifiers" Width="130" Margin="0,0,5,0"/>
                            <TextBlock Text="+" VerticalAlignment="Center" Margin="5,0"/>
                            <ComboBox x:Name="cmbKey" Width="80" Margin="0,0,10,0"/>
                            <Button x:Name="btnTestHotkey" 
                                    Content="Test" 
                                    Padding="15,5"
                                    Click="BtnTestHotkey_Click"/>
                        </StackPanel>
                        
                        <TextBlock x:Name="txtHotkeyStatus" 
                                   Text="Status: Ready"
                                   FontSize="12"
                                   Foreground="{StaticResource SecondaryTextBrush}"
                                   Margin="0,5,0,0"/>
                    </StackPanel>
                </GroupBox>
                
                <!-- Schedule Section -->
                <GroupBox Header="Schedule" Margin="0,15,0,0">
                    <StackPanel Margin="10">
                        <CheckBox x:Name="chkEnableSchedule" 
                                  Content="Enable automatic schedule"
                                  Margin="0,0,0,10"/>
                        
                        <Grid IsEnabled="{Binding ElementName=chkEnableSchedule, Path=IsChecked}"
                              Opacity="{Binding ElementName=chkEnableSchedule, Path=IsChecked, Converter={StaticResource BooleanToOpacityConverter}}">
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="Auto"/>
                                <ColumnDefinition Width="*"/>
                            </Grid.ColumnDefinitions>
                            <Grid.RowDefinitions>
                                <RowDefinition Height="Auto"/>
                                <RowDefinition Height="Auto"/>
                                <RowDefinition Height="Auto"/>
                            </Grid.RowDefinitions>
                            
                            <Label Grid.Row="0" Grid.Column="0" Content="Days:"/>
                            <ComboBox x:Name="cmbScheduleDays" Grid.Row="0" Grid.Column="1">
                                <ComboBoxItem Content="Monday - Friday"/>
                                <ComboBoxItem Content="Every Day"/>
                                <ComboBoxItem Content="Weekends Only"/>
                            </ComboBox>
                            
                            <Label Grid.Row="1" Grid.Column="0" Content="Start Time:"/>
                            <StackPanel Grid.Row="1" Grid.Column="1" Orientation="Horizontal">
                                <ComboBox x:Name="cmbStartHour" Width="60"/>
                                <TextBlock Text=":" VerticalAlignment="Center" Margin="2,0"/>
                                <ComboBox x:Name="cmbStartMinute" Width="60"/>
                            </StackPanel>
                            
                            <Label Grid.Row="2" Grid.Column="0" Content="End Time:"/>
                            <StackPanel Grid.Row="2" Grid.Column="1" Orientation="Horizontal">
                                <ComboBox x:Name="cmbEndHour" Width="60"/>
                                <TextBlock Text=":" VerticalAlignment="Center" Margin="2,0"/>
                                <ComboBox x:Name="cmbEndMinute" Width="60"/>
                            </StackPanel>
                        </Grid>
                    </StackPanel>
                </GroupBox>
                
                <!-- Options Section -->
                <GroupBox Header="Options" Margin="0,15,0,0">
                    <StackPanel Margin="10">
                        <CheckBox x:Name="chkAutoStartup" 
                                  Content="Start with Windows"
                                  Margin="0,0,0,8"/>
                        
                        <CheckBox x:Name="chkStartHidden" 
                                  Content="Start hidden in system tray"
                                  Margin="0,0,0,8"/>
                        
                        <StackPanel Orientation="Horizontal" Margin="0,5,0,0">
                            <Label Content="Jiggle Interval:"/>
                            <ComboBox x:Name="cmbInterval" Width="100">
                                <ComboBoxItem Content="500 ms"/>
                                <ComboBoxItem Content="1000 ms" IsSelected="True"/>
                                <ComboBoxItem Content="2000 ms"/>
                                <ComboBoxItem Content="5000 ms"/>
                            </ComboBox>
                        </StackPanel>
                    </StackPanel>
                </GroupBox>
                
            </StackPanel>
        </ScrollViewer>
        
        <!-- Buttons -->
        <StackPanel Grid.Row="2" 
                    Orientation="Horizontal" 
                    HorizontalAlignment="Right"
                    Margin="0,15,0,0">
            <Button x:Name="btnSave" 
                    Content="Save" 
                    Click="BtnSave_Click"
                    Margin="0,0,10,0"/>
            <Button x:Name="btnCancel" 
                    Content="Cancel" 
                    Click="BtnCancel_Click"
                    Background="{StaticResource HoverBrush}"/>
        </StackPanel>
        
    </Grid>
</Window>'''

def main():
    parser = argparse.ArgumentParser(description='Generate settings dialog')
    parser.add_argument('--output', required=True, help='Output file path')
    args = parser.parse_args()
    
    with open(args.output, 'w') as f:
        f.write(SETTINGS_XAML)
    
    print(f"✓ Generated settings dialog: {args.output}")

if __name__ == '__main__':
    main()
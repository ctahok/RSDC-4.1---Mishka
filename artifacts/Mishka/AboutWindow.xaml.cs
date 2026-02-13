using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace Mishka
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }
        
        private void LnkGitHub_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/ctahok/RSDC-4.1-Mishka",
                    UseShellExecute = true
                });
            }
            catch
            {
                System.Windows.MessageBox.Show("Could not open browser.\n\nPlease visit:\nhttps://github.com/ctahok/RSDC-4.1-Mishka",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
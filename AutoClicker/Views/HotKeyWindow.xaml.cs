using System.Windows;

namespace AutoClicker.Views
{
    /// <summary>
    /// Interaction logic for HotKeyWindow.xaml
    /// </summary>
    public partial class HotKeyWindow : Window
    {
        public HotKeyWindow()
        {
            InitializeComponent();
        }

        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

using System;
using System.Windows;
using System.Windows.Interop;
using WpfTest.Models.Hotkeys;

namespace WpfTest.Views.Main
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private HwndSource? _source;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr handle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(handle)!;
            _source.AddHook(GlobalHotKey.HwndHook);

            GlobalHotKey.RegisterHotKey(handle);
        }
    }
}
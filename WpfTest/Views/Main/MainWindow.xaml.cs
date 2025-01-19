using Microsoft.VisualBasic;
using System;
using System.Windows;
using System.Windows.Interop;
using WpfTest.Models.Hotkeys;
using WpfTest.Services;

namespace WpfTest.Views.Main
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private HwndSource? _source;

        #region Life cycle

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr handle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(handle)!;
            _source.AddHook(GlobalHotKey.HwndHook);

            GlobalHotKey.RegisterHotKey(handle);
        }

        protected override void OnClosed(EventArgs e)
        {
            _source!.RemoveHook(GlobalHotKey.HwndHook);

            ChildWindowsService.CloseAll();

            base.OnClosed(e);
        }

        #endregion
    }
}
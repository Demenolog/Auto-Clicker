using AutoClicker.Infrastructure.Constants.HotkeysClass;
using AutoClicker.Infrastructure.UnsafeCode;
using AutoClicker.Models.Hotkeys;
using AutoClicker.Services;
using AutoClicker.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Interop;

namespace AutoClicker.Views.Main
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private HwndSource? _source;

        #region Life cycle

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr handle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(handle)!;
            _source.AddHook(GlobalHotKey.HwndHook);

            GlobalHotKey.RegisterHotKeys(handle);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_source is not null)
            {
                _source.RemoveHook(GlobalHotKey.HwndHook);
            }

            var mouseClicker = App.Services.GetRequiredService<IMouseClicker>();
            if (mouseClicker.IsRunning)
            {
                mouseClicker.StopClicking();
            }

            var handle = new WindowInteropHelper(this).Handle;
            if (handle != IntPtr.Zero)
            {
                User32.UnregisterHotKey(handle, GlobalHotKeyConstance.START_HOTKEY_ID);
                User32.UnregisterHotKey(handle, GlobalHotKeyConstance.STOP_HOTKEY_ID);
            }

            ChildWindowsService.CloseAll();

            base.OnClosed(e);
        }

        #endregion

    }
}

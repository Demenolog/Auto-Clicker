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
        public MainWindow()
        {
            InitializeComponent();
            StateChanged += OnStateChanged;
            Closing += OnClosing;
        }

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

        internal void CleanupForExit()
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
        }

        #endregion

        private void OnStateChanged(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized && IsMinimizeToTrayEnabled())
            {
                Hide();
            }
        }

        private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsMinimizeToTrayEnabled() || App.IsExitRequested)
            {
                return;
            }

            e.Cancel = true;
            Hide();
        }

        private static bool IsMinimizeToTrayEnabled()
        {
            var settingsService = App.Services.GetRequiredService<ISettingsService>();
            return settingsService.Settings.MinimizeToTray;
        }
    }
}

using AutoClicker;
using AutoClicker.Infrastructure.Constants.HotkeysClass;
using AutoClicker.Infrastructure.UnsafeCode;
using AutoClicker.Services.Interfaces;
using AutoClicker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using System.Windows.Input;
using static AutoClicker.Infrastructure.Constants.HotkeysClass.GlobalHotKeyConstance;

namespace AutoClicker.Models.Hotkeys
{
    internal static class GlobalHotKey
    {
        private static readonly ViewModelLocator Locator = new();
        private static IMouseClicker MouseClicker => App.Services.GetRequiredService<IMouseClicker>();
        private static IntPtr s_handle;
        private static HotKeyDefinition? s_registeredStartHotKey;
        private static HotKeyDefinition? s_registeredStopHotKey;
        internal static readonly HotKeyDefinition DefaultStartHotKey = new(ModifierKeys.None, Key.F3);
        internal static readonly HotKeyDefinition DefaultStopHotKey = new(ModifierKeys.None, Key.F4);

        public static void ChangeHotKeys()
        {
            var previousStart = s_registeredStartHotKey;
            var previousStop = s_registeredStopHotKey;

            UnregisterHotKeys();

            var startBinding = Locator.HotKeyWindowModel.StartHotKey;
            var stopBinding = Locator.HotKeyWindowModel.StopHotKey;

            if (TryRegisterBindings(startBinding, stopBinding, notifyOnFailure: true))
            {
                return;
            }

            RestorePreviousBindings(previousStart, previousStop);
            if (previousStart is not null && previousStop is not null)
            {
                TryRegisterBindings(previousStart, previousStop, notifyOnFailure: false);
            }
        }

        private static (uint Modifiers, uint VirtualKey) GetHotKeyRegistration(HotKeyDefinition hotKey)
        {
            var virtualKey = (uint)KeyInterop.VirtualKeyFromKey(hotKey.Key);
            uint flags = MOD_NONE;

            if (hotKey.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                flags |= MOD_ALT;
            }

            if (hotKey.Modifiers.HasFlag(ModifierKeys.Control))
            {
                flags |= MOD_CONTROL;
            }

            if (hotKey.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                flags |= MOD_SHIFT;
            }

            if (hotKey.Modifiers.HasFlag(ModifierKeys.Windows))
            {
                flags |= MOD_WIN;
            }

            return (flags, virtualKey);
        }

        public static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case START_HOTKEY_ID:
                            uint startModifiers = (uint)((int)lParam & 0xFFFF);
                            uint vkey = (uint)(((int)lParam >> 16) & 0xFFFF);
                            var startBinding = Locator.HotKeyWindowModel.StartHotKey;
                            var startRegistration = GetHotKeyRegistration(startBinding);

                            if (vkey == startRegistration.VirtualKey
                                && startModifiers == startRegistration.Modifiers
                                && !MouseClicker.IsRunning)
                            {
                                Locator.MainWindowModel.OnStartClickingExecute(null);
                            }
                            handled = true;
                            break;
                        case STOP_HOTKEY_ID:
                            uint stopModifiers = (uint)((int)lParam & 0xFFFF);
                            uint stopVKey = (uint)(((int)lParam >> 16) & 0xFFFF);
                            var stopBinding = Locator.HotKeyWindowModel.StopHotKey;
                            var stopRegistration = GetHotKeyRegistration(stopBinding);

                            if (stopVKey == stopRegistration.VirtualKey
                                && stopModifiers == stopRegistration.Modifiers
                                && MouseClicker.IsRunning)
                            {
                                Locator.MainWindowModel.OnStopClickingExecute(null);
                            }

                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        public static void RegisterHotKeys(IntPtr handle)
        {
            s_handle = handle;

            Registration();
        }

        public static void ResetHotKeys()
        {
            var previousStart = s_registeredStartHotKey;
            var previousStop = s_registeredStopHotKey;

            UnregisterHotKeys();

            Locator.HotKeyWindowModel.SetStartHotKey(DefaultStartHotKey);
            Locator.HotKeyWindowModel.SetStopHotKey(DefaultStopHotKey);

            if (TryRegisterBindings(DefaultStartHotKey, DefaultStopHotKey, notifyOnFailure: true))
            {
                return;
            }

            RestorePreviousBindings(previousStart, previousStop);
            if (previousStart is not null && previousStop is not null)
            {
                TryRegisterBindings(previousStart, previousStop, notifyOnFailure: false);
            }
        }

        private static void Registration()
        {
            var startBinding = Locator.HotKeyWindowModel.StartHotKey;
            var stopBinding = Locator.HotKeyWindowModel.StopHotKey;

            TryRegisterBindings(startBinding, stopBinding, notifyOnFailure: true);
        }

        private static void UnregisterHotKeys()
        {
            if (s_handle == IntPtr.Zero)
            {
                return;
            }

            User32.UnregisterHotKey(s_handle, START_HOTKEY_ID);
            User32.UnregisterHotKey(s_handle, STOP_HOTKEY_ID);
        }

        private static bool TryRegisterBindings(HotKeyDefinition startBinding, HotKeyDefinition stopBinding, bool notifyOnFailure)
        {
            if (!TryRegisterHotKeys(s_handle, startBinding, stopBinding, User32.RegisterHotKey, User32.UnregisterHotKey,
                    NotifyRegistrationFailure, LogRegistrationWarning, notifyOnFailure))
            {
                return false;
            }

            s_registeredStartHotKey = startBinding;
            s_registeredStopHotKey = stopBinding;
            return true;
        }

        private static void RestorePreviousBindings(HotKeyDefinition? previousStart, HotKeyDefinition? previousStop)
        {
            if (previousStart is null || previousStop is null)
            {
                return;
            }

            Locator.HotKeyWindowModel.SetStartHotKey(previousStart);
            Locator.HotKeyWindowModel.SetStopHotKey(previousStop);
        }

        private static void NotifyRegistrationFailure(string message)
        {
            MessageBox.Show(message, HotKeyMessages.RegistrationFailureTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private static void LogRegistrationWarning(string message)
        {
            var loggerFactory = App.Services.GetService<ILoggerFactory>();
            var logger = loggerFactory?.CreateLogger(nameof(GlobalHotKey));
            logger?.LogWarning(message);
        }

        private static string BuildFailureLogMessage(string hotKeyName, HotKeyDefinition binding, (uint Modifiers, uint VirtualKey) registration)
        {
            return $"Failed to register {hotKeyName} hotkey {binding.ToDisplayString()} (Modifiers: {registration.Modifiers}, VirtualKey: {registration.VirtualKey}).";
        }

        internal static bool TryRegisterHotKeys(
            IntPtr handle,
            HotKeyDefinition startBinding,
            HotKeyDefinition stopBinding,
            Func<nint, int, uint, uint, bool> registerHotKey,
            Func<nint, int, bool> unregisterHotKey,
            Action<string> notifyUser,
            Action<string> logWarning,
            bool notifyOnFailure)
        {
            var startRegistration = GetHotKeyRegistration(startBinding);
            if (!registerHotKey(handle, START_HOTKEY_ID, startRegistration.Modifiers, startRegistration.VirtualKey))
            {
                logWarning(BuildFailureLogMessage("Start", startBinding, startRegistration));
                if (notifyOnFailure)
                {
                    notifyUser(HotKeyMessages.HotkeyAlreadyInUse);
                }
                return false;
            }

            var stopRegistration = GetHotKeyRegistration(stopBinding);
            if (!registerHotKey(handle, STOP_HOTKEY_ID, stopRegistration.Modifiers, stopRegistration.VirtualKey))
            {
                unregisterHotKey(handle, START_HOTKEY_ID);
                logWarning(BuildFailureLogMessage("Stop", stopBinding, stopRegistration));
                if (notifyOnFailure)
                {
                    notifyUser(HotKeyMessages.HotkeyAlreadyInUse);
                }
                return false;
            }

            return true;
        }
    }
}

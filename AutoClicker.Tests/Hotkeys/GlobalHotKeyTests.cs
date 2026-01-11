using System;
using AutoClicker.Infrastructure.Constants.HotkeysClass;
using AutoClicker.Models.Hotkeys;
using System.Windows.Input;
using Xunit;

namespace AutoClicker.Tests.Hotkeys
{
    public class GlobalHotKeyTests
    {
        [Fact]
        public void TryRegisterHotKeys_WhenStartRegistrationFails_NotifiesAndSkipsUnregister()
        {
            var startBinding = new HotKeyDefinition(ModifierKeys.None, Key.F3);
            var stopBinding = new HotKeyDefinition(ModifierKeys.None, Key.F4);
            var notifiedMessage = string.Empty;
            var logMessage = string.Empty;
            var unregisterCalled = false;

            var result = GlobalHotKey.TryRegisterHotKeys(
                IntPtr.Zero,
                startBinding,
                stopBinding,
                (_, _, _, _) => false,
                (_, _) =>
                {
                    unregisterCalled = true;
                    return true;
                },
                message => notifiedMessage = message,
                message => logMessage = message,
                notifyOnFailure: true);

            Assert.False(result);
            Assert.Equal(HotKeyMessages.HotkeyAlreadyInUse, notifiedMessage);
            Assert.Contains("Start hotkey", logMessage, StringComparison.OrdinalIgnoreCase);
            Assert.False(unregisterCalled);
        }

        [Fact]
        public void TryRegisterHotKeys_WhenStopRegistrationFails_UnregistersStartAndNotifies()
        {
            var startBinding = new HotKeyDefinition(ModifierKeys.None, Key.F3);
            var stopBinding = new HotKeyDefinition(ModifierKeys.None, Key.F4);
            var notifiedMessage = string.Empty;
            var logMessage = string.Empty;
            var unregisterCalledWithStart = false;
            var callCount = 0;

            var result = GlobalHotKey.TryRegisterHotKeys(
                IntPtr.Zero,
                startBinding,
                stopBinding,
                (_, _, _, _) => ++callCount == 1,
                (_, id) =>
                {
                    if (id == GlobalHotKeyConstance.START_HOTKEY_ID)
                    {
                        unregisterCalledWithStart = true;
                    }
                    return true;
                },
                message => notifiedMessage = message,
                message => logMessage = message,
                notifyOnFailure: true);

            Assert.False(result);
            Assert.True(unregisterCalledWithStart);
            Assert.Equal(HotKeyMessages.HotkeyAlreadyInUse, notifiedMessage);
            Assert.Contains("Stop hotkey", logMessage, StringComparison.OrdinalIgnoreCase);
        }
    }
}

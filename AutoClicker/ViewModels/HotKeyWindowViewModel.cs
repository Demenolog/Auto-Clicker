using System.Windows.Input;
using AutoClicker.Infrastructure.Commands;
using AutoClicker.Models.Hotkeys;
using AutoClicker.ViewModels.Base;

namespace AutoClicker.ViewModels
{
    internal class HotKeyWindowViewModel : ViewModel
    {
        #region StartHotkey : string - definition for textbox with start hotkey

        private string _startHotkey = GlobalHotKey.DefaultStartHotKey;

        public string StartHotkey
        {
            get => _startHotkey;
            set => SetField(ref _startHotkey, value);
        }

        #endregion StartHotkey : string - definition for textbox with start hotkey

        #region StopHotKey : string - Definition for textbox with stop hotkey

        private string _stopHotkey = GlobalHotKey.DefaultStopHotKey;

        public string StopHotKey
        {
            get => _stopHotkey;
            set => SetField(ref _stopHotkey, value);
        }

        #endregion StopHotKey : string - Definition for textbox with stop hotkey

        #region Accept command

        public ICommand ChangeHotKeys { get; }

        private bool CanChangeHotKeysExecuted(object p) => true;

        private void OnChangeHotKeysExecute(object p)
        {
            GlobalHotKey.ChangeHotKeys();
        }

        #endregion Accept command

        #region Reset Hotkeys command

        public ICommand ResetHotKeys { get; }

        private bool CanResetHotKeysExecuted(object p) => true;

        private void OnResetHotKeysExecute(object p)
        {
            GlobalHotKey.ResetHotKeys();
        }

        #endregion Reset Hotkeys command

        public HotKeyWindowViewModel()
        {
            ChangeHotKeys = new LambdaCommand(OnChangeHotKeysExecute, CanChangeHotKeysExecuted);

            ResetHotKeys = new LambdaCommand(OnResetHotKeysExecute, CanResetHotKeysExecuted);
        }
    }
}
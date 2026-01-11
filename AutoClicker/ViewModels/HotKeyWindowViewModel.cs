using System.Windows.Input;
using AutoClicker.Infrastructure.Commands;
using AutoClicker.Models.Hotkeys;
using AutoClicker.ViewModels.Base;

namespace AutoClicker.ViewModels
{
    internal class HotKeyWindowViewModel : ViewModel
    {
        #region StartHotKey : string - definition for textbox with start hotkey

        private HotKeyBinding _startHotKeyBinding = GlobalHotKey.DefaultStartHotKey;
        private string _startHotKey = GlobalHotKey.DefaultStartHotKey.ToDisplayString();

        public HotKeyBinding StartHotKeyBinding
        {
            get => _startHotKeyBinding;
            private set => SetField(ref _startHotKeyBinding, value);
        }

        public string StartHotKey
        {
            get => _startHotKey;
            private set => SetField(ref _startHotKey, value);
        }

        public void SetStartHotKey(HotKeyBinding binding)
        {
            StartHotKeyBinding = binding;
            StartHotKey = binding.ToDisplayString();
        }

        #endregion StartHotKey : string - definition for textbox with start hotkey

        #region StopHotKey : string - Definition for textbox with stop hotkey

        private HotKeyBinding _stopHotKeyBinding = GlobalHotKey.DefaultStopHotKey;
        private string _stopHotkey = GlobalHotKey.DefaultStopHotKey.ToDisplayString();

        public HotKeyBinding StopHotKeyBinding
        {
            get => _stopHotKeyBinding;
            private set => SetField(ref _stopHotKeyBinding, value);
        }

        public string StopHotKey
        {
            get => _stopHotkey;
            private set => SetField(ref _stopHotkey, value);
        }

        public void SetStopHotKey(HotKeyBinding binding)
        {
            StopHotKeyBinding = binding;
            StopHotKey = binding.ToDisplayString();
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

        public ICommand ResetHotKeys { get; }

        private bool CanResetHotKeysExecuted(object p) => true;

        private void OnResetHotKeysExecute(object p)
        {
            GlobalHotKey.ResetHotKeys();
        }


        public HotKeyWindowViewModel()
        {
            ChangeHotKeys = new LambdaCommand(OnChangeHotKeysExecute, CanChangeHotKeysExecuted);

            ResetHotKeys = new LambdaCommand(OnResetHotKeysExecute, CanResetHotKeysExecuted);

            SetStartHotKey(GlobalHotKey.DefaultStartHotKey);
            SetStopHotKey(GlobalHotKey.DefaultStopHotKey);
        }
    }
}

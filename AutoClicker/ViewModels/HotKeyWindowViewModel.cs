using System.Windows.Input;
using AutoClicker.Infrastructure.Commands;
using AutoClicker.Models.Hotkeys;
using AutoClicker.ViewModels.Base;

namespace AutoClicker.ViewModels
{
    internal class HotKeyWindowViewModel : ViewModel
    {
        #region StartHotKey : definition for textbox with start hotkey

        private HotKeyDefinition _startHotKey = GlobalHotKey.DefaultStartHotKey;

        public HotKeyDefinition StartHotKey
        {
            get => _startHotKey;
            private set
            {
                if (SetField(ref _startHotKey, value))
                {
                    OnPropertyChanged(nameof(StartHotKeyDisplay));
                }
            }
        }

        public string StartHotKeyDisplay => StartHotKey.ToDisplayString();

        public void SetStartHotKey(HotKeyDefinition binding)
        {
            StartHotKey = binding;
        }

        #endregion StartHotKey : definition for textbox with start hotkey

        #region StopHotKey : definition for textbox with stop hotkey

        private HotKeyDefinition _stopHotKey = GlobalHotKey.DefaultStopHotKey;

        public HotKeyDefinition StopHotKey
        {
            get => _stopHotKey;
            private set
            {
                if (SetField(ref _stopHotKey, value))
                {
                    OnPropertyChanged(nameof(StopHotKeyDisplay));
                }
            }
        }

        public string StopHotKeyDisplay => StopHotKey.ToDisplayString();

        public void SetStopHotKey(HotKeyDefinition binding)
        {
            StopHotKey = binding;
        }

        #endregion StopHotKey : definition for textbox with stop hotkey

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

using System.Windows;
using System.Windows.Input;
using AutoClicker.Infrastructure.Commands;
using AutoClicker.Models.Hotkeys;
using AutoClicker.Services.Interfaces;
using AutoClicker.Services.Settings;
using AutoClicker.ViewModels.Base;

namespace AutoClicker.ViewModels
{
    internal class HotKeyWindowViewModel : ViewModel
    {
        private readonly ISettingsService _settingsService;
        private bool _isLoadingSettings;

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
            UpdateSettings(settings =>
            {
                settings.HotKeys.StartModifiers = binding.Modifiers;
                settings.HotKeys.StartKey = binding.Key;
            });
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
            UpdateSettings(settings =>
            {
                settings.HotKeys.StopModifiers = binding.Modifiers;
                settings.HotKeys.StopKey = binding.Key;
            });
        }

        #endregion StopHotKey : definition for textbox with stop hotkey

        #region Accept command

        public ICommand ChangeHotKeys { get; }

        private bool CanChangeHotKeysExecuted(object p) => true;

        private void OnChangeHotKeysExecute(object p)
        {
            if (StartHotKey == StopHotKey)
            {
                MessageBox.Show("Start and stop hotkeys must be different.", "Hotkey conflict",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GlobalHotKey.ChangeHotKeys();
        }

        #endregion Accept command

        public ICommand ResetHotKeys { get; }

        private bool CanResetHotKeysExecuted(object p) => true;

        private void OnResetHotKeysExecute(object p)
        {
            GlobalHotKey.ResetHotKeys();
        }


        public HotKeyWindowViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            ChangeHotKeys = new LambdaCommand(OnChangeHotKeysExecute, CanChangeHotKeysExecuted);

            ResetHotKeys = new LambdaCommand(OnResetHotKeysExecute, CanResetHotKeysExecuted);

            ApplySettings(_settingsService.Settings);
        }

        private void ApplySettings(AppSettings settings)
        {
            _isLoadingSettings = true;
            var hotKeys = settings.HotKeys ?? new HotKeySettings();
            var startHotKey = new HotKeyDefinition(hotKeys.StartModifiers, hotKeys.StartKey);
            var stopHotKey = new HotKeyDefinition(hotKeys.StopModifiers, hotKeys.StopKey);
            SetStartHotKey(startHotKey);
            SetStopHotKey(stopHotKey);
            _isLoadingSettings = false;
        }

        private void UpdateSettings(System.Action<AppSettings> updateAction)
        {
            if (_isLoadingSettings)
            {
                return;
            }

            _settingsService.Update(updateAction);
        }
    }
}

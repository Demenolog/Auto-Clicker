using AutoClicker.Infrastructure.Commands;
using AutoClicker.Models.Clicks;
using AutoClicker.Models.Other;
using AutoClicker.Services.Interfaces;
using AutoClicker.Services.Settings;
using AutoClicker.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoClicker.Services;
using Point = System.Drawing.Point;

namespace AutoClicker.ViewModels
{
    internal enum RepeatMode
    {
        UntilStopped,
        Times
    }

    internal enum PositionMode
    {
        Current,
        Fixed
    }

    internal class MainWindowViewModel : ViewModel
    {
        private readonly ISettingsService _settingsService;
        private bool _isLoadingSettings;

        #region [Cilick interval]

        #region Properties

        #region HoursTextBox : string - TextBox with hours values

        private string _hours = "0";

        public string HoursTextBox
        {
            get => _hours;
            set
            {
                if (TextBoxValidation.IsPositiveIntNumber(value))
                {
                    if (SetField(ref _hours, value))
                    {
                        UpdateSettings(settings => settings.Hours = value);
                    }
                }
            }
        }

        #endregion HoursTextBox : string - TextBox with hours values

        #region MinutesTextBox : string - TextBox with minutes values

        private string _minutes = "0";

        public string MinutesTextBox
        {
            get => _minutes;
            set
            {
                if (TextBoxValidation.IsPositiveIntNumber(value))
                {
                    if (SetField(ref _minutes, value))
                    {
                        UpdateSettings(settings => settings.Minutes = value);
                    }
                }
            }
        }

        #endregion MinutesTextBox : string - TextBox with minutes values

        #region SecondsTextBox : string - TextBox with seconds values

        private string _secondsTextBox = "1";

        public string SecondsTextBox
        {
            get => _secondsTextBox;
            set
            {
                if (TextBoxValidation.IsPositiveIntNumber(value))
                {
                    if (SetField(ref _secondsTextBox, value))
                    {
                        UpdateSettings(settings => settings.Seconds = value);
                    }
                }
            }
        }

        #endregion SecondsTextBox : string - TextBox with seconds values

        #region MillisecondsTextBox : string - TextBox with msec values

        private string _milliseconds = "0";

        public string MillisecondsTextBox
        {
            get => _milliseconds;
            set
            {
                if (TextBoxValidation.IsPositiveIntNumber(value))
                {
                    if (SetField(ref _milliseconds, value))
                    {
                        UpdateSettings(settings => settings.Milliseconds = value);
                    }
                }
            }
        }

        #endregion MillisecondsTextBox : string - TextBox with msec values

        #endregion Properties

        #endregion [Cilick interval]

        #region [Click options]

        #region Properties

        #region Selected Mouse Button : MouseButtonKind - Selected mouse button from combobox

        private MouseButtonKind _mouseButton = MouseButtonKind.Left;

        public Array MouseButtonOptions { get; } = Enum.GetValues(typeof(MouseButtonKind));

        public MouseButtonKind SelectedMouseButton
        {
            get => _mouseButton;
            set
            {
                if (SetField(ref _mouseButton, value))
                {
                    UpdateSettings(settings => settings.SelectedMouseButton = value);
                }
            }
        }

        #endregion Selected Mouse Button : MouseButtonKind - Selected mouse button from combobox

        #region Selected Mouse Button Mode : ClickBurstKind - Selected click type from combobox

        private ClickBurstKind _selectedMouseButtonMode = ClickBurstKind.Single;

        public Array ClickBurstOptions { get; } = Enum.GetValues(typeof(ClickBurstKind));

        public ClickBurstKind SelectedMouseButtonMode
        {
            get => _selectedMouseButtonMode;
            set
            {
                if (SetField(ref _selectedMouseButtonMode, value))
                {
                    UpdateSettings(settings => settings.SelectedMouseButtonMode = value);
                }
            }
        }

        #endregion Selected Mouse Button Mode : ClickBurstKind - Selected click type from combobox

        #endregion Properties

        #endregion [Click options]

        #region [Click repeat]

        #region Properties

        #region RepeatTimesTextBox : string - get repeat times amount

        private string _repeatTimes = "1";

        public string RepeatTimesTextBox
        {
            get => _repeatTimes;
            set
            {
                if (TextBoxValidation.IsPositiveIntNumber(value))
                {
                    if (SetField(ref _repeatTimes, value))
                    {
                        UpdateSettings(settings => settings.RepeatTimes = value);
                    }
                }
            }
        }

        #endregion RepeatTimesTextBox : string - get repeat times amount

        #region Repeat Mode : RepeatMode - selected repeat mode

        private RepeatMode _repeatMode = RepeatMode.UntilStopped;

        public RepeatMode RepeatMode
        {
            get => _repeatMode;
            set
            {
                if (SetField(ref _repeatMode, value))
                {
                    UpdateSettings(settings => settings.RepeatUntilStopped = value == RepeatMode.UntilStopped);
                }
            }
        }

        #endregion Repeat Mode : RepeatMode - selected repeat mode

        #endregion Properties

        #endregion [Click repeat]

        #region [Cursor position]

        #region Properties

        #region Position Mode : PositionMode - selected cursor position mode

        private PositionMode _positionMode = PositionMode.Current;

        public PositionMode PositionMode
        {
            get => _positionMode;
            set
            {
                if (SetField(ref _positionMode, value))
                {
                    UpdateSettings(settings => settings.PositionUseCurrent = value == PositionMode.Current);
                }
            }
        }

        #endregion Position Mode : PositionMode - selected cursor position mode

        #region XAxisTextBox : string - Get\Set text value of X-axis textBox

        private string _xAxis = "0";

        public string XAxisTextBox
        {
            get => _xAxis;
            set
            {
                if (TextBoxValidation.IsIntNumber(value))
                {
                    if (SetField(ref _xAxis, value))
                    {
                        UpdateSettings(settings => settings.LastX = value);
                    }
                }
            }
        }

        #endregion XAxisTextBox : string - Get\Set text value of X-axis textBox

        #region YAxisTextBox : string - Get\Set text value from Y-axis textBox

        private string _yAxis = "0";

        public string YAxisTextBox
        {
            get => _yAxis;
            set
            {
                if (TextBoxValidation.IsIntNumber(value))
                {
                    if (SetField(ref _yAxis, value))
                    {
                        UpdateSettings(settings => settings.LastY = value);
                    }
                }
            }
        }

        #endregion YAxisTextBox : string - Get\Set text value from Y-axis textBox

        #region IsPickLockationEnable

        #region IsPickLocationBtnEnable : bool - checked if pick location button is enable

        private bool _isPickLocationBtnEnable = true;

        public bool IsPickLocationBtnEnable
        {
            get => _isPickLocationBtnEnable;
            set => SetField(ref _isPickLocationBtnEnable, value);
        }

        #endregion IsPickLocationBtnEnable : bool - checked if pick location button is enable

        #endregion IsPickLockationEnable

        #endregion Properties

        #region Commands

        #region Get cursor position command

        public ICommand GetCursorPosition { get; }

        private bool CanGetCursorPositionExecuted(object p) => true;

        private async void OnGetCursorPositionExecute(object p)
        {
            IsPickLocationBtnEnable = false;

            try
            {
                var result = await Task.Run(() =>
                {
                    var success = _mouseClicker.TryGetCursorPosition(out var point);
                    return (success, point);
                });

                if (!result.success)
                {
                    return;
                }

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    XAxisTextBox = result.point.X.ToString();
                    YAxisTextBox = result.point.Y.ToString();
                });
            }
            finally
            {
                IsPickLocationBtnEnable = true;
            }
        }

        #endregion Get cursor position command

        #endregion Commands

        #endregion [Cursor position]

        #region [Buttons section]

        #region Commands

        #region Start clicking command

        public ICommand StartClicking { get; }

        private bool CanStartClickingExecuted(object p) => !IsRunning;

        internal async void OnStartClickingExecute(object p)
        {
            try
            {
                if (PositionMode == PositionMode.Fixed && !IsFixedPositionWithinVirtualScreen())
                {
                    MessageBox.Show("Fixed X/Y position is outside the virtual screen bounds.",
                        "AutoClicker Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var config = BuildClickConfig();
                var click = new Click(config);

                IsRunning = true;
                await _mouseClicker.StartClicking(click);
            }
            catch (Exception ex)
            {
                _mouseClicker.StopClicking();
                IsRunning = false;
                MessageBox.Show($"Unable to start clicking. {ex.Message}", "AutoClicker Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion Start clicking command

        #region Stop clicking command

        public ICommand StopClicking { get; }

        private bool CanStopClickingExecuted(object p) => IsRunning;

        internal void OnStopClickingExecute(object p)
        {
            IsRunning = false;
            _mouseClicker.StopClicking();
        }

        #endregion Stop clicking command

        #region Open hotKeys Window

        public ICommand OpenHotKeysWindow { get; }

        private bool CanOpenHotKeysWindowExecuted(object p) => true;

        private void OnOpenHotKeysWindowExecute(object p)
        {
            HotKeysWindowService.Create();

            if (!HotKeysWindowService.Show())
            {
                return;
            }
        }

        #endregion Open hotKeys Window

        #endregion Commands

        #endregion [Buttons section]

        #region [Running state]

        private bool _isRunning;

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (SetField(ref _isRunning, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        #endregion [Running state]

        private readonly IMouseClicker _mouseClicker;

        public MainWindowViewModel(IMouseClicker mouseClicker, ISettingsService settingsService)
        {
            _mouseClicker = mouseClicker;
            _settingsService = settingsService;
            ApplySettings(_settingsService.Settings);
            StartClicking = new LambdaCommand(OnStartClickingExecute, CanStartClickingExecuted);

            StopClicking = new LambdaCommand(OnStopClickingExecute, CanStopClickingExecuted);

            GetCursorPosition = new LambdaCommand(OnGetCursorPositionExecute, CanGetCursorPositionExecuted);

            OpenHotKeysWindow = new LambdaCommand(OnOpenHotKeysWindowExecute, CanOpenHotKeysWindowExecuted);

            _mouseClicker.ClickingStopped += OnClickingStopped;
        }

        private void OnClickingStopped()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsRunning = false;
            });
        }

        private ClickConfig BuildClickConfig()
        {
            var position = PositionMode == PositionMode.Current
                ? _mouseClicker.GetCurrentCursorPosition()
                : new Point(ParseAxisValue(XAxisTextBox), ParseAxisValue(YAxisTextBox));

            return new ClickConfig(
                new ClickIntervalConfig(
                    HoursTextBox,
                    MinutesTextBox,
                    SecondsTextBox,
                    MillisecondsTextBox),
                new ClickOptionsConfig(
                    SelectedMouseButton,
                    SelectedMouseButtonMode),
                new ClickRepeatsConfig(
                    RepeatMode == RepeatMode.UntilStopped,
                    RepeatTimesTextBox),
                new ClickPositionConfig(
                    PositionMode == PositionMode.Current,
                    position.X.ToString(),
                    position.Y.ToString()));
        }

        private bool IsFixedPositionWithinVirtualScreen()
        {
            var x = ParseAxisValue(XAxisTextBox);
            var y = ParseAxisValue(YAxisTextBox);

            var left = SystemParameters.VirtualScreenLeft;
            var top = SystemParameters.VirtualScreenTop;
            var right = left + SystemParameters.VirtualScreenWidth;
            var bottom = top + SystemParameters.VirtualScreenHeight;

            return x >= left && x < right && y >= top && y < bottom;
        }

        private static int ParseAxisValue(string value)
        {
            return int.TryParse(value, out var axis) ? axis : 0;
        }

        private void ApplySettings(AppSettings settings)
        {
            _isLoadingSettings = true;
            HoursTextBox = settings.Hours ?? "0";
            MinutesTextBox = settings.Minutes ?? "0";
            SecondsTextBox = settings.Seconds ?? "1";
            MillisecondsTextBox = settings.Milliseconds ?? "0";
            SelectedMouseButton = settings.SelectedMouseButton;
            SelectedMouseButtonMode = settings.SelectedMouseButtonMode;
            RepeatTimesTextBox = settings.RepeatTimes ?? "1";
            RepeatMode = settings.RepeatUntilStopped ? RepeatMode.UntilStopped : RepeatMode.Times;
            PositionMode = settings.PositionUseCurrent ? PositionMode.Current : PositionMode.Fixed;
            XAxisTextBox = settings.LastX ?? "0";
            YAxisTextBox = settings.LastY ?? "0";
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

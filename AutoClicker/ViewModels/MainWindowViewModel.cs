using AutoClicker.Infrastructure.Commands;
using AutoClicker.Models.MouseClass;
using AutoClicker.Models.Other;
using AutoClicker.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Point = System.Drawing.Point;

namespace AutoClicker.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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
                    SetField(ref _hours, value);
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
                    SetField(ref _minutes, value);
                }
            }
        }

        #endregion MinutesTextBox : string - TextBox with minutes values

        #region SecondsTextBox : string - TextBox with seconds values

        private string _secondsTextBox = "0";

        public string SecondsTextBox
        {
            get => _secondsTextBox;
            set
            {
                if (TextBoxValidation.IsPositiveIntNumber(value))
                {
                    SetField(ref _secondsTextBox, value);
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
                    SetField(ref _milliseconds, value);
                }
            }
        }

        #endregion MillisecondsTextBox : string - TextBox with msec values

        #endregion Properties

        #endregion [Cilick interval]

        #region [Click options]

        #region Properties

        #region Selected Mouse Button : string - Selected mouse button from combobox

        private string _mouseButton = "Left";

        public string SelectedMouseButton
        {
            get => _mouseButton;
            set => SetField(ref _mouseButton, value);
        }

        #endregion Selected Mouse Button : string - Selected mouse button from combobox

        #region Selected Mouse Button Mode : string - Selected click type from combobox

        private string _selectedMouseButtonMode = "Single";

        public string SelectedMouseButtonMode
        {
            get => _selectedMouseButtonMode;
            set => SetField(ref _selectedMouseButtonMode, value);
        }

        #endregion Selected Mouse Button Mode : string - Selected click type from combobox

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
                    SetField(ref _repeatTimes, value);
                }
            }
        }

        #endregion RepeatTimesTextBox : string - get repeat times amount

        #region Is Repeat Times Selected : bool - checking if repeat checkbox selected

        private bool _isRepeatTimes;

        public bool IsRepeatTimesSelected
        {
            get => _isRepeatTimes;
            set
            {
                SetField(ref _isRepeatTimes, value);
                SetField(ref _isRepeatUntilStopped, !_isRepeatTimes);
                OnPropertyChanged(nameof(IsRepeatUntilStoppedSelected));
            }
        }

        #endregion Is Repeat Times Selected : bool - checking if repeat checkbox selected

        #region Is Repeat Until Stopped Selected : bool - checking if repeat until stopped checkbox selected

        private bool _isRepeatUntilStopped = true;

        public bool IsRepeatUntilStoppedSelected
        {
            get => _isRepeatUntilStopped;
            set
            {
                SetField(ref _isRepeatUntilStopped, value);
                SetField(ref _isRepeatTimes, !_isRepeatUntilStopped);
                OnPropertyChanged(nameof(IsRepeatTimesSelected));
            }
        }

        #endregion Is Repeat Until Stopped Selected : bool - checking if repeat until stopped checkbox selected

        #endregion Properties

        #endregion [Click repeat]

        #region [Cursor position]

        #region Properties

        #region IsCurrentLocationSelected : bool - checking if current location checkbox selected

        private bool _isCurrentLocation = true;

        public bool IsCurrentLocationSelected
        {
            get => _isCurrentLocation;
            set
            {
                SetField(ref _isCurrentLocation, value);
                SetField(ref _isPickLocation, !_isCurrentLocation);
                OnPropertyChanged(nameof(IsPickLocationSelected));
            }
        }

        #endregion IsCurrentLocationSelected : bool - checking if current location checkbox selected

        #region IsPickLocationSelected : bool - checking if pick location checkbox selected

        private bool _isPickLocation;

        public bool IsPickLocationSelected
        {
            get => _isPickLocation;
            set
            {
                SetField(ref _isPickLocation, value);
                SetField(ref _isCurrentLocation, !_isPickLocation);
                OnPropertyChanged(nameof(IsCurrentLocationSelected));
            }
        }

        #endregion IsPickLocationSelected : bool - checking if pick location checkbox selected

        #region XAxisTextBox : string - Get\Set text value of X-axis textBox

        private string _xAxis = "0";

        public string XAxisTextBox
        {
            get => _xAxis;
            set
            {
                if (TextBoxValidation.IsIntNumber(value))
                {
                    SetField(ref _xAxis, value);
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
                    SetField(ref _yAxis, value);
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
                await Task.Run((() =>
                {
                    var point = MouseClicks.GetCursorPosition();

                    XAxisTextBox = point.X.ToString();
                    YAxisTextBox = point.Y.ToString();
                }));
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

        private bool CanStartClickingExecuted(object p) => true;

        private async void OnStartClickingExecute(object p)
        {
            try
            {
                // Get interval amount of time # 1

                var intervalTime =
                    IntervalCounter.GetTotalIntervalTime(HoursTextBox, MinutesTextBox, SecondsTextBox, MillisecondsTextBox);

                // Get mouse button and click mode options # 2

                var selectedButton = SelectedMouseButton;
                var selectedButtonMode = MouseClicks.GetClickMode(SelectedMouseButtonMode);

                // Get click repeat mode # 3

                var repeatMode = IsRepeatUntilStoppedSelected ? -1 : int.Parse(RepeatTimesTextBox); //  ???

                // Get Cursor position # 4

                var cursorPosition = IsCurrentLocationSelected ? MouseClicks.GetCurrentCursorPosition() : new Point(int.Parse(XAxisTextBox), int.Parse(YAxisTextBox));

                // Run a task\thread # 5
                
                await MouseClicks.StartClicking(intervalTime, selectedButton, selectedButtonMode, repeatMode, cursorPosition);
            }
            finally
            {
            }
        }

        #endregion Start clicking command

        #region Stop clicking command

        public ICommand StopClicking { get; }

        private bool CanStopClickingExecuted(object p) => MouseClicks.Cts != null;

        private void OnStopClickingExecute(object p)
        {
            MouseClicks.StopClicking();
        }

        #endregion Stop clicking command

        #endregion Commands

        #endregion [Buttons section]

        public MainWindowViewModel()
        {
            StartClicking = new LambdaCommand(OnStartClickingExecute, CanStartClickingExecuted);

            StopClicking = new LambdaCommand(OnStopClickingExecute, CanStopClickingExecuted);

            GetCursorPosition = new LambdaCommand(OnGetCursorPositionExecute, CanGetCursorPositionExecuted);
        }
    }
}
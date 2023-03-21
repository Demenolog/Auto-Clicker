﻿using AutoClicker.Models.Other;
using AutoClicker.ViewModels.Base;

namespace AutoClicker.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region [Cilick interval]

        #region Fileds

        #region HoursTextBox : string - TextBox with hours values

        private string _hours = "0";

        public string HoursTextBox
        {
            get => _hours;
            set
            {
                if (TextBoxValidation.IsIntegerNumber(value))
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
                if (TextBoxValidation.IsIntegerNumber(value))
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
                if (TextBoxValidation.IsIntegerNumber(value))
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
                if (TextBoxValidation.IsIntegerNumber(value))
                {
                    SetField(ref _milliseconds, value);
                }
            }
        }

        #endregion MillisecondsTextBox : string - TextBox with msec values

        #endregion Fileds

        #endregion [Cilick interval]

        #region [Click options]

        #region Fields

        #region Selected Mouse Button : string - Selected button from combobox

        private string _mouseButton = "Left";

        public string SelectedMouseButton
        {
            get => _mouseButton;
            set => SetField(ref _mouseButton, value);
        }

        #endregion Selected Mouse Button : string - Selected button from combobox

        #region Selected Mouse Button Mode : string - Selected click type from combobox

        private string _selectedMouseButtonMode = "Single";

        public string SelectedMouseButtonMode
        {
            get => _selectedMouseButtonMode;
            set => SetField(ref _selectedMouseButtonMode, value);
        }

        #endregion Selected Mouse Button Mode : string - Selected click type from combobox

        #endregion Fields

        #endregion [Click options]

        #region [Click repeat]

        #region Fields

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

        #endregion Fields

        #endregion [Click repeat]

        #region [Cursor position]

        #region Fields

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

        #region XAxisTextBox : double - Get\Set text value of X-axis textBox

        private double _xAxis;

        public double XAxisTextBox
        {
            get => _xAxis;
            set => SetField(ref _xAxis, value);
        }

        #endregion

        #region YAxisTextBox : double - Get\Set text value from Y-axis textBox

        private double _yAxis;

        public double YAxisTextBox
        {
            get => _yAxis;
            set => SetField(ref _yAxis, value);
        }

        #endregion

        #endregion Fields

        #endregion [Cursor position]

        public MainWindowViewModel()
        {
        }
    }
}
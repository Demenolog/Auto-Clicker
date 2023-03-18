using AutoClicker.Models.Other;
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
                if (TextBoxValidation.IsNumber(value))
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
                if (TextBoxValidation.IsNumber(value))
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
                if (TextBoxValidation.IsNumber(value))
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
                if (TextBoxValidation.IsNumber(value))
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

        public MainWindowViewModel()
        {
        }
    }
}
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
            set => SetField(ref _hours, value);
        }

        #endregion

        #region MinutesTextBox : string - TextBox with minutes values

        private string _minutes = "0";

        public string MinutesTextBox
        {
            get => _minutes;
            set => SetField(ref _minutes, value);
        }

        #endregion

        #region SecondsTextBox : string - TextBox with seconds values

        private string _secondsTextBox = "0";

        public string SecondsTextBox
        {
            get => _secondsTextBox;
            set => SetField(ref _secondsTextBox, value);
        }

        #endregion

        #region MillisecondsTextBox : string - TextBox with msec values

        private string _milliseconds = "0";

        public string MillisecondsTextBox
        {
            get => _milliseconds;
            set => SetField(ref _milliseconds, value);
        }

        #endregion

        #endregion

        #endregion
    }
}
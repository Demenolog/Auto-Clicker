using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;

namespace AutoClicker.Models.States
{
    internal class Repeats
    {
        private readonly int _repeats;
        private static readonly MainWindowViewModel MainWindow = ViewModelLocatorProvider.MainWindow;

        public Repeats()
        {
            _repeats = GetRepeats();
        }

        public int TotalTimes => _repeats;

        private int GetRepeats()
        {
            var isEndless = MainWindow.IsRepeatUntilStoppedSelected;

            if (isEndless)
            {
                return -1;
            }

            var text = MainWindow.RepeatTimesTextBox;

            if (!int.TryParse(text, out var times))
            {
                return 0;
            }

            if (times < 0)
            {
                times = 0;
            }

            return times;
        }
    }
}

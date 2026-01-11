using AutoClicker.Models.Clicks;

namespace AutoClicker.Models.States
{
    internal class Repeats
    {
        private readonly int _repeats;

        public Repeats(ClickRepeatsConfig config)
        {
            _repeats = GetRepeats(config);
        }

        public int TotalTimes => _repeats;

        private int GetRepeats(ClickRepeatsConfig config)
        {
            var isEndless = config.IsRepeatUntilStopped;

            if (isEndless)
            {
                return -1;
            }

            var text = config.RepeatTimes;

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

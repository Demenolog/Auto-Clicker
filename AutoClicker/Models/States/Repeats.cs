using AutoClicker.Models.Clicks;
using AutoClicker.Models.Parsing;

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
            return RepeatParser.ParseRepeatCount(config.IsRepeatUntilStopped, config.RepeatTimes);
        }
    }
}

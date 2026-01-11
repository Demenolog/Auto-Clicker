using AutoClicker.Models.Clicks.States;
using AutoClicker.Models.States;
using Options = AutoClicker.Models.States.Options;

namespace AutoClicker.Models.Clicks
{
    internal class Click
    {
        private readonly Interval _interval;
        private readonly Options _options;
        private readonly Repeats _repeats;
        private readonly Position _position;

        public Click(ClickConfig config)
        {
            _interval = new Interval(config.Interval);
            _options = new Options(config.Options);
            _repeats = new Repeats(config.Repeats);
            _position = new Position(config.Position);
        }

        public Interval Interval => _interval;

        public Options Options => _options;

        public Repeats Repeats => _repeats;

        public Position Position => _position;
    }
}

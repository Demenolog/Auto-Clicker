using System;
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
        private readonly TimeSpan _startDelay;
        private readonly TimeSpan _stopAfter;

        public Click(ClickConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _interval = new Interval(config.Interval);
            _options = new Options(config.Options);
            _repeats = new Repeats(config.Repeats);
            _position = new Position(config.Position);
            _startDelay = config.StartDelay;
            _stopAfter = config.StopAfter;
        }

        public Interval Interval => _interval;

        public Options Options => _options;

        public Repeats Repeats => _repeats;

        public Position Position => _position;

        public TimeSpan StartDelay => _startDelay;

        public TimeSpan StopAfter => _stopAfter;
    }
}

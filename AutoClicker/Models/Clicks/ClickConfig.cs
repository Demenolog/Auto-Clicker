namespace AutoClicker.Models.Clicks
{
    internal sealed class ClickConfig
    {
        public ClickConfig(
            ClickIntervalConfig interval,
            ClickOptionsConfig options,
            ClickRepeatsConfig repeats,
            ClickPositionConfig position)
        {
            Interval = interval;
            Options = options;
            Repeats = repeats;
            Position = position;
        }

        public ClickIntervalConfig Interval { get; }

        public ClickOptionsConfig Options { get; }

        public ClickRepeatsConfig Repeats { get; }

        public ClickPositionConfig Position { get; }
    }

    internal sealed class ClickIntervalConfig
    {
        public ClickIntervalConfig(string hours, string minutes, string seconds, string milliseconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }

        public string Hours { get; }

        public string Minutes { get; }

        public string Seconds { get; }

        public string Milliseconds { get; }
    }

    internal sealed class ClickOptionsConfig
    {
        public ClickOptionsConfig(MouseButtonKind button, ClickBurstKind buttonMode)
        {
            Button = button;
            ButtonMode = buttonMode;
        }

        public MouseButtonKind Button { get; }

        public ClickBurstKind ButtonMode { get; }
    }

    internal sealed class ClickRepeatsConfig
    {
        public ClickRepeatsConfig(bool isRepeatUntilStopped, string repeatTimes)
        {
            IsRepeatUntilStopped = isRepeatUntilStopped;
            RepeatTimes = repeatTimes;
        }

        public bool IsRepeatUntilStopped { get; }

        public string RepeatTimes { get; }
    }

    internal sealed class ClickPositionConfig
    {
        public ClickPositionConfig(bool isCurrentLocationSelected, string xAxis, string yAxis)
        {
            IsCurrentLocationSelected = isCurrentLocationSelected;
            XAxis = xAxis;
            YAxis = yAxis;
        }

        public bool IsCurrentLocationSelected { get; }

        public string XAxis { get; }

        public string YAxis { get; }
    }
}

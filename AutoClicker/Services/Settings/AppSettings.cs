using AutoClicker.Models.Clicks;

namespace AutoClicker.Services.Settings
{
    internal sealed class AppSettings
    {
        public string Hours { get; set; } = "0";
        public string Minutes { get; set; } = "0";
        public string Seconds { get; set; } = "1";
        public string Milliseconds { get; set; } = "0";
        public MouseButtonKind SelectedMouseButton { get; set; } = MouseButtonKind.Left;
        public ClickBurstKind SelectedMouseButtonMode { get; set; } = ClickBurstKind.Single;
        public bool RepeatUntilStopped { get; set; } = true;
        public string RepeatTimes { get; set; } = "1";
        public bool PositionUseCurrent { get; set; } = true;
        public string LastX { get; set; } = "0";
        public string LastY { get; set; } = "0";
        public HotKeySettings HotKeys { get; set; } = new();
    }
}

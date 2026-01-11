using System.Windows.Input;

namespace AutoClicker.Services.Settings
{
    internal sealed class HotKeySettings
    {
        public ModifierKeys StartModifiers { get; set; } = ModifierKeys.None;
        public Key StartKey { get; set; } = Key.F3;
        public ModifierKeys StopModifiers { get; set; } = ModifierKeys.None;
        public Key StopKey { get; set; } = Key.F4;
    }
}

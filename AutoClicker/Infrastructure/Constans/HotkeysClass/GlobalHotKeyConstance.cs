namespace AutoClicker.Infrastructure.Constans.HotkeysClass
{
    public static class GlobalHotKeyConstance
    {
        public const int HOTKEY_ID = 9000;
        public const uint MOD_NONE = 0x0000; //[NONE]
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
        public const int WM_HOTKEY = 0x0312;
    }
}

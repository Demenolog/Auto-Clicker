using WpfTest.Views;

namespace WpfTest.Services
{
    internal static class HotKeysWindowService
    {
        private static HotKeyWindow s_hotKeyWindow = null;

        public static bool IsCreated => s_hotKeyWindow != null;

        public static HotKeyWindow HotKeyWindow
        {
            get => s_hotKeyWindow;
            set => s_hotKeyWindow = value;
        }

        public static void Create()
        {
            if (HotKeyWindow != null) return;

            HotKeyWindow = new HotKeyWindow();
            HotKeyWindow.Closed += (o, args) => HotKeyWindow = null;

            ChildWindowsService.Add(HotKeyWindow);
        }

        public static bool Show()
        {
            if (HotKeyWindow != null)
            {
                HotKeyWindow.Show();
                return true;
            }

            return false;
        }
    }
}
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AutoClicker.Services
{
    internal static class ChildWindowsService
    {
        public static ObservableCollection<Window> ChildWindows { get; } = new ObservableCollection<Window>();

        public static void Add(Window window)
        {
            if (!ChildWindows.Contains(window))
            {
                window.Closed -= WindowOnClosed;
                window.Closed += WindowOnClosed;
                ChildWindows.Add(window);
            }
        }

        public static void Remove(Window window)
        {
            if (ChildWindows.Contains(window))
            {
                ChildWindows.Remove(window);
            }

            window.Closed -= WindowOnClosed;
        }

        private static void WindowOnClosed(object sender, System.EventArgs e)
        {
            if (sender is Window window)
            {
                window.Closed -= WindowOnClosed;
                Remove(window);
            }
        }

        public static void CloseAll()
        {
            foreach (var window in ChildWindows.ToList())
            {
                window.Close();
            }
        }
    }
}

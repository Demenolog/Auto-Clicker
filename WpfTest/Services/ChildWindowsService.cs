using System.Collections.ObjectModel;
using System.Windows;

namespace WpfTest.Services
{
    internal static class ChildWindowsService
    {
        public static ObservableCollection<Window> ChildWindows { get; } = new ObservableCollection<Window>();

        public static void Add(Window window)
        {
            if (!ChildWindows.Contains(window))
            {
                ChildWindows.Add(window);
            }
        }

        public static void Remove(Window window)
        {
            if (ChildWindows.Contains(window))
            {
                ChildWindows.Remove(window);
            }
        }

        public static void CloseAll()
        {
            foreach (var window in ChildWindows)
            {
                window.Close();
            }
        }
    }
}
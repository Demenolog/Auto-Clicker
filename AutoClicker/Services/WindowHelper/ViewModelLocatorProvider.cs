using AutoClicker.ViewModels;

namespace AutoClicker.Services.WindowHelper
{
    internal static class ViewModelLocatorProvider
    {
        public static readonly MainWindowViewModel MainWindow = new ViewModelLocator().MainWindowModel;

        public static readonly HotKeyWindowViewModel HotKeyWindow = new ViewModelLocator().HotKeyWindowModel;
    }
}
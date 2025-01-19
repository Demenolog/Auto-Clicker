using Microsoft.Extensions.DependencyInjection;

namespace AutoClicker.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();

        public HotKeyWindowViewModel HotKeyWindowModel => App.Services.GetRequiredService<HotKeyWindowViewModel>();
    }
}
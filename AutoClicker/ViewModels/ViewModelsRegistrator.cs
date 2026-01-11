using AutoClicker.Services.Interfaces;
using AutoClicker.Services.MouseClicker;
using Microsoft.Extensions.DependencyInjection;

namespace AutoClicker.ViewModels
{
    internal static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModel(this IServiceCollection services) => services
            .AddSingleton<IMouseClicker, MouseClicker>()
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<HotKeyWindowViewModel>();
    }
}

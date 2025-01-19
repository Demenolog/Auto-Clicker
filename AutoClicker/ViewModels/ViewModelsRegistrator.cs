using Microsoft.Extensions.DependencyInjection;

namespace AutoClicker.ViewModels
{
    internal static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModel(this IServiceCollection services) => services
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<HotKeyWindowViewModel>();
    }
}
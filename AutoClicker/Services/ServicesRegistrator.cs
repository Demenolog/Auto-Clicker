using AutoClicker.Services.Interfaces;
using AutoClicker.Services.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace AutoClicker.Services
{
    internal static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient<IUserDialog, UserDialogService>()
            .AddSingleton<ISettingsService, SettingsService>();
    }
}

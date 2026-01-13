using AutoClicker.Services;
using AutoClicker.Services.Interfaces;
using AutoClicker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace AutoClicker
{
    public partial class App
    {
        private static IHost? s_host;
        private static bool s_exitRequested;

        public static IServiceProvider Services => Host.Services;

        public static IHost Host => s_host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddServices();
            services.AddViewModel();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;

            var settingsService = host.Services.GetRequiredService<ISettingsService>();
            settingsService.Load();

            base.OnStartup(e);

            await host.StartAsync();

            var trayIconService = host.Services.GetRequiredService<ITrayIconService>();
            trayIconService.Initialize();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            var settingsService = Host.Services.GetRequiredService<ISettingsService>();
            settingsService.Save();

            if (Current.MainWindow is Views.Main.MainWindow mainWindow)
            {
                mainWindow.CleanupForExit();
            }

            var trayIconService = Host.Services.GetRequiredService<ITrayIconService>();
            trayIconService.Dispose();

            base.OnExit(e);

            using (Host)
            {
                await Host.StopAsync();
            }
        }

        internal static bool IsExitRequested => s_exitRequested;

        internal static void RequestExit()
        {
            s_exitRequested = true;
        }
    }
}

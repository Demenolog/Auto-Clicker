using Microsoft.Extensions.Hosting;
using mrousavy;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace WpfTest
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(App.ConfigureServices);
    }
}
﻿using Microsoft.Extensions.Hosting;
using System;
using TestWpf.ViewModels;

namespace TestWpf
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
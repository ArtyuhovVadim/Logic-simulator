using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogicSimulator;

public partial class App
{
    private static IHost host;

    public static IHost Host => host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static bool IsDesignMode { get; private set; } = true;

    public static string CurrentDirectory =>
        IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath()) : Environment.CurrentDirectory;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IsDesignMode = false;
        
        await Host.StartAsync().ConfigureAwait(false);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        await Host.StopAsync().ConfigureAwait(false);
        host.Dispose();
    }

    public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services
            .RegisterViewModels()
            .RegisterServices();
    }

    private static string GetSourceCodePath([CallerFilePath] string path = null)
    {
        return path;
    }
}
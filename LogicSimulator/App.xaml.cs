using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using LogicSimulator.Controls.Themes;
using LogicSimulator.Controls.Themes.Dark;
using LogicSimulator.Controls.Themes.Light;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogicSimulator;

public partial class App
{
    private static IHost host;

    private static ThemeType currentTheme = ThemeType.Dark;

    private static readonly Dictionary<ThemeType, Theme> Themes = new()
    {
        { ThemeType.Dark, new DarkTheme() },
        { ThemeType.Light, new LightTheme() }
    };

    public static IHost Host => host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static bool IsDesignMode { get; private set; } = true;

    public static string CurrentDirectory =>
        IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath()) : Environment.CurrentDirectory;

    public static ThemeType CurrentTheme
    {
        get => currentTheme;
        set
        {
            if (currentTheme == value) return;

            var resources = Current.Resources.MergedDictionaries;

            var oldTheme = GetCurrentResourceDictionary();
            var oldThemeIndex = resources.IndexOf(oldTheme);

            resources.RemoveAt(oldThemeIndex);
            resources.Insert(oldThemeIndex, new ResourceDictionary { Source = Themes[value].GetResourceUri() });

            currentTheme = value;
        }
    }

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

    private static ResourceDictionary GetCurrentResourceDictionary()
    {
        var resourceDictionary = Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.OriginalString.EndsWith("Theme.xaml"));

        if (resourceDictionary is null)
            throw new ApplicationException("Theme resource not found");

        return resourceDictionary;
    }
}
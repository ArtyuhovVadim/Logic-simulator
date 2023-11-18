﻿using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using LogicSimulator.Controls.Themes;
using LogicSimulator.Controls.Themes.Dark;
using LogicSimulator.Controls.Themes.Light;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.ViewModels.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogicSimulator;

public partial class App
{
    private static IHost _host;

    private static ThemeType _currentTheme = ThemeType.Dark;

    private static readonly Dictionary<ThemeType, Theme> Themes = new()
    {
        { ThemeType.Dark, new DarkTheme() },
        { ThemeType.Light, new LightTheme() }
    };

    public static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static Window ActiveWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

    public static Window FocusedWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsFocused);

    public static Window CurrentWindow => FocusedWindow ?? ActiveWindow;

    public static bool IsDesignMode { get; private set; } = true;

    public static string CurrentDirectory =>
        IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath()) : Environment.CurrentDirectory;

    public static ThemeType CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme == value) return;

            var resources = Current.Resources.MergedDictionaries;

            var oldTheme = GetCurrentResourceDictionary();
            var oldThemeIndex = resources.IndexOf(oldTheme);

            resources.RemoveAt(oldThemeIndex);
            resources.Insert(oldThemeIndex, new ResourceDictionary { Source = Themes[value].GetResourceUri() });

            _currentTheme = value;
        }
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IsDesignMode = false;

        Environment.CurrentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()!.Location)!;

        await Host.StartAsync().ConfigureAwait(false);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        await Host.StopAsync().ConfigureAwait(false);
        _host.Dispose();
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
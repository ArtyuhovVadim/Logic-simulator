using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels;
using LogicSimulator.ViewModels.UserDialogViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogicSimulator;

public partial class App
{
    private static IHost _host;

    public static IHost Host => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static Window ActiveWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

    public static Window FocusedWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsFocused);

    public static Window CurrentWindow => FocusedWindow ?? ActiveWindow;

    public static bool IsDesignMode { get; private set; } = true;

    public static string CurrentDirectory => IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath()) : Environment.CurrentDirectory;

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

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<PropertiesViewModel>()
            .AddSingleton<ProjectExplorerViewModel>()
            .AddSingleton<InfoDialogWindowViewModel>()
            .AddSingleton<ErrorDialogWindowViewModel>()
            .AddSingleton<WarningDialogWindowViewModel>()
            .AddSingleton<QuestionDialogWindowViewModel>()

            .AddSingleton<IUserDialogService, UserDialogService>()
            .AddSingleton<ISchemeFileService, SchemeFileService>()
            .AddSingleton<IProjectFileService, ProjectFileService>()
            .AddSingleton<IEditorSelectionService, EditorSelectionService>();
    }

    private static string GetSourceCodePath([CallerFilePath] string path = null) => path;

    private static IHostBuilder CreateHostBuilder(string[] args) => Microsoft.Extensions.Hosting.Host
        .CreateDefaultBuilder(args)
        .UseContentRoot(CurrentDirectory)
        .ConfigureAppConfiguration((_, cfg) => cfg
            .SetBasePath(CurrentDirectory)
            .AddJsonFile("app-settings.json", true, true))
        .ConfigureServices(ConfigureServices);
}
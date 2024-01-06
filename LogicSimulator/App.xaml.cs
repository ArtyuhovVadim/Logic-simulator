using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels;
using LogicSimulator.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogicSimulator;

public partial class App
{
    private static readonly IHost Host = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static Window? ActiveWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

    public static Window? FocusedWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsFocused);

    public static Window? CurrentWindow => FocusedWindow ?? ActiveWindow;

    public static bool IsDesignMode { get; private set; } = true;

    public static string CurrentDirectory => IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath())! : Environment.CurrentDirectory;

    protected override async void OnStartup(StartupEventArgs args)
    {
        base.OnStartup(args);

        IsDesignMode = false;

        Environment.CurrentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()!.Location)!;

#if !DEBUG
        SetupGlobalExceptionHandling();
#endif

        await Host.StartAsync().ConfigureAwait(false);

        Host.Services.GetRequiredService<MainWindow>().Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        await Host.StopAsync().ConfigureAwait(false);
        Host.Dispose();
    }

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<DockingViewModel>()
            .AddSingleton<PropertiesViewModel>()
            .AddSingleton<ProjectExplorerViewModel>()

            .AddSingleton<MainWindow>(serviceProvider => new MainWindow { DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>() })

            .AddSingleton<IUserDialogService, DefaultUserDialogService>()
            .AddSingleton<ISchemeFileService, SchemeFileService>()
            .AddSingleton<IProjectFileService, ProjectFileService>()
            .AddSingleton<IEditorSelectionService, EditorSelectionService>()

            .AddSingleton<ISchemeViewModelFactory, SchemeViewModelFactory>()
            .AddSingleton<IProjectViewModelFactory, ProjectViewModelFactory>()
            ;
    }

    private static void SetupGlobalExceptionHandling()
    {
        try
        {
            // handles non-UI thread exceptions thrown; the app terminates after unhandled exceptions are caught here
            AppDomain.CurrentDomain.UnhandledException += (_, e) => HandleException((Exception)e.ExceptionObject, e.IsTerminating);

            // handles UI dispatcher thread exceptions thrown
            Current.DispatcherUnhandledException += (_, e) => e.Handled = HandleException(e.Exception);

            // handles domain-wide exceptions where a task scheduler is used for asynchronous operations
            TaskScheduler.UnobservedTaskException += (_, e) => { if (HandleException(e.Exception)) e.SetObserved(); };
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Unable to use global exception handling.\n{e.Message}");
        }
    }

    private static bool HandleException(Exception exception, bool isTerminating = false)
    {
        if (isTerminating)
        {
            MessageBox.Show(exception.ToString(), "Critical error", MessageBoxButton.OK, MessageBoxImage.Error);

            Current.Shutdown();

            return false;
        }

        MessageBox.Show(exception.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        return true;
    }

    private static string GetSourceCodePath([CallerFilePath] string? path = null) => path!;

    private static IHostBuilder CreateHostBuilder(string[] args) => Microsoft.Extensions.Hosting.Host
        .CreateDefaultBuilder(args)
        .UseContentRoot(CurrentDirectory)
        .ConfigureAppConfiguration((_, cfg) => cfg
            .SetBasePath(CurrentDirectory)
            .AddJsonFile("app-settings.json", true, true))
        .ConfigureServices(ConfigureServices);
}
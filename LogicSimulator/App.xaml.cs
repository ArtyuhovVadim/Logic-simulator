using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using LogicSimulator.Infrastructure.Factories;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Logging;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.Logic.Gates;
using LogicSimulator.Models.Objects;
using LogicSimulator.Models.Objects.Base;
using LogicSimulator.ViewModels;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Logic;
using LogicSimulator.ViewModels.Logic.Gates;
using LogicSimulator.ViewModels.Objects;
using LogicSimulator.ViewModels.Objects.Base;
using LogicSimulator.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WpfExtensions.Mvvm.Messaging;

namespace LogicSimulator;

public partial class App
{
    private static readonly IHost Host = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static Version Version { get; private set; } = null!;

    public static Window? ActiveWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

    public static Window? FocusedWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsFocused);

    public static Window? CurrentWindow => FocusedWindow ?? ActiveWindow;

    public static bool IsDesignMode { get; private set; } = true;

#if DEBUG
    public static bool IsDevelopment => true;

    public static bool IsRelease => false;
#else
    public static bool IsDevelopment => false;

    public static bool IsRelease => true;
#endif

    public static string CurrentDirectory => IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath())! : Environment.CurrentDirectory;

    protected override async void OnStartup(StartupEventArgs args)
    {
        base.OnStartup(args);

        IsDesignMode = false;

        Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        Version = Assembly.GetEntryAssembly()?.GetName().Version!;

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
            .AddSingleton<MessagesOutputViewModel>()
            .AddSingleton<TimelineViewModel>()

            .AddSingleton<MainWindow>(serviceProvider => new MainWindow { DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>() })

            .AddSingleton<IUserDialogService, DefaultUserDialogService>()
            .AddSingleton<ISchemeFileService, SchemeFileService>()
            .AddSingleton<IProjectFileService, ProjectFileService>()
            .AddSingleton<IEditorSelectionService, EditorSelectionService>()
            .AddSingleton<IOutputMessagesService, OutputMessagesService>()
            .AddSingleton<IClipboardService, ClipboardService>()
            .AddSingleton<IMessageBus, MessageBus>()

            .AddTransient<ISchemePreprocessorService, SchemePreprocessorService>()
            .AddTransient<ISchemeValidationService, SchemeValidationService>()
            .AddTransient<ISchemeBuilderService, SchemeBuilderService>()
            .AddTransient<ISchemeSimulatorService, SchemeSimulatorService>()
            .AddTransient<IToolSwitcherService, ToolSwitcherService>()

            .AddSingleton<ISchemeViewModelFactory, SchemeViewModelFactory>()
            .AddSingleton<IProjectViewModelFactory, ProjectViewModelFactory>()
            .AddSingleton<IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel>>(_ =>
            {
                var factory = new SchemeObjectViewModelFactory();
                factory.Register<ArcModel>(model => new ArcViewModel(model));
                factory.Register<BezierCurveModel>(model => new BezierCurveViewModel(model));
                factory.Register<EllipseModel>(model => new EllipseViewModel(model));
                factory.Register<LineModel>(model => new LineViewModel(model));
                factory.Register<RectangleModel>(model => new RectangleViewModel(model));
                factory.Register<RoundedRectangleModel>(model => new RoundedRectangleViewModel(model));
                factory.Register<TextBlockModel>(model => new TextBlockViewModel(model));
                factory.Register<PathModel>(model => new PathViewModel(model));

                factory.Register<InputGateModel>(model => new InputGateViewModel(model));
                factory.Register<OutputGateModel>(model => new OutputGateViewModel(model));
                factory.Register<AndGateModel>(model => new AndGateViewModel(model));
                factory.Register<WireModel>(model => new WireViewModel(model));
                return factory;
            });
    }

    private static void ConfigureLogging(ILoggingBuilder builder)
    {
        builder
            .ClearProviders()
            .AddConsole()
#if DEBUG
            .AddDebug()
            .Services.AddSingleton<ILoggerProvider, OutputMessagesLoggerProvider>()
#endif
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
                                                                             .ConfigureLogging(ConfigureLogging)
                                                                             .ConfigureServices(ConfigureServices);
}
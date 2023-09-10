#if !DEBUG
using System.Windows;
using LogicSimulator.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LogicSimulator;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
#if DEBUG
        var app = new App();
        app.InitializeComponent();
        app.Run();
#else
        try
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
        catch (Exception e)
        {
            var dialog = App.Host.Services.GetRequiredService<IUserDialogService>();

            var message = $"Message: {e.Message}\nSource: {e.Source}\nStackTrace:\n{e.StackTrace}";

            if (dialog is null)
            {
                MessageBox.Show(message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                dialog.ShowErrorMessage("Ошибка!", message);
            }
        }
#endif
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseContentRoot(App.CurrentDirectory)
            .ConfigureAppConfiguration((host, cfg) => cfg
                .SetBasePath(App.CurrentDirectory)
                .AddJsonFile("app-settings.json", true, true))
            .ConfigureServices(App.ConfigureServices);
    }
}
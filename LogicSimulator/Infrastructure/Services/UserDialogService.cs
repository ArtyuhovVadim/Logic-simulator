using System.Windows;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models.Common;
using LogicSimulator.ViewModels.Dialog.Base;
using LogicSimulator.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace LogicSimulator.Infrastructure.Services;

public class UserDialogService : IUserDialogService
{
    private readonly IServiceProvider _provider;

    public UserDialogService(IServiceProvider provider) => _provider = provider;

    public T ShowDialog<T>() where T : BaseDialogViewModel => ShowDialogInternal<T>(null);

    public T ShowDialog<T>(Action<T> configure) where T : BaseDialogViewModel => ShowDialogInternal(configure);

    private T ShowDialogInternal<T>(Action<T>? configure) where T : BaseDialogViewModel => Application.Current.Dispatcher.Invoke(() =>
    {
        var dataContext = _provider.GetRequiredService<T>();
        configure?.Invoke(dataContext);

        var window = new DialogWindow { DataContext = dataContext, Owner = Application.Current.MainWindow };

        dataContext.Completed += () => window.Close();

        window.ShowDialog();

        return dataContext;
    });

    public UserDialogResult OpenFileDialog(string title, IEnumerable<(string name, string pattern)> filters, out string path)
    {
        path = string.Empty;

        var filter = string.Join('|', filters.Select(x => $"{x.name} ({x.pattern})|{x.pattern}"));
        var openFileDialog = new OpenFileDialog { Title = title, Filter = filter };

        if (openFileDialog.ShowDialog() == true)
        {
            path = openFileDialog.FileName;
            return UserDialogResult.Ok;
        }

        return UserDialogResult.Cancel;
    }

    public UserDialogResult OpenFolderDialog(string title, out string path)
    {
        path = string.Empty;

        var openFileDialog = new OpenFolderDialog { Title = title };

        if (openFileDialog.ShowDialog() == true)
        {
            path = openFileDialog.FolderName;
            return UserDialogResult.Ok;
        }

        return UserDialogResult.Cancel;
    }
}
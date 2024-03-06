using System.Windows;
using LogicSimulator.Infrastructure.Services.Interfaces;
using Microsoft.Win32;

namespace LogicSimulator.Infrastructure.Services;

public class DefaultUserDialogService : IUserDialogService
{
    public UserDialogResult ShowInfoMessage(string title, string message)
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        return result == MessageBoxResult.OK ? UserDialogResult.Ok : UserDialogResult.None;
    }

    public UserDialogResult ShowErrorMessage(string title, string message)
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        return result == MessageBoxResult.OK ? UserDialogResult.Ok : UserDialogResult.None;
    }

    public UserDialogResult ShowWarningMessage(string title, string message)
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        return result == MessageBoxResult.OK ? UserDialogResult.Ok : UserDialogResult.None;
    }

    public UserDialogResult ShowQuestionMessage(string title, string message)
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Question);

        return result switch
        {
            MessageBoxResult.Yes => UserDialogResult.Yes,
            MessageBoxResult.No => UserDialogResult.No,
            _ => UserDialogResult.None
        };
    }

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
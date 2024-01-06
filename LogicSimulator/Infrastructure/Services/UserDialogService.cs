using System.Windows;
using LogicSimulator.Infrastructure.Services.Interfaces;

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
}
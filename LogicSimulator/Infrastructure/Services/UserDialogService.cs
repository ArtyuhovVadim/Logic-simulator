using System.Windows;
using LogicSimulator.ViewModels.UserDialogViewModels;
using LogicSimulator.Views.Windows.UserDialogWindows;

namespace LogicSimulator.Infrastructure.Services;

public interface IUserDialogService
{
    void ShowInfoMessage(string message);

    void ShowErrorMessage(string message);

    void ShowWarningMessage(string message);

    void ShowQuestionMessage(string message);
}

public class UserDialogService : IUserDialogService
{
    private readonly InfoDialogWindowViewModel _infoDialogWindowViewModel;

    public UserDialogService(InfoDialogWindowViewModel infoDialogWindowViewModel)
    {
        _infoDialogWindowViewModel = infoDialogWindowViewModel;
    }

    public void ShowInfoMessage(string message)
    {
        _infoDialogWindowViewModel.Message = message;

        var infoDialogWindow = new InfoDialogWindow
        {
            DataContext = _infoDialogWindowViewModel,
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        _infoDialogWindowViewModel.Completed += () =>
        {
            infoDialogWindow.Close();
        };

        infoDialogWindow.ShowDialog();
    }

    public void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public void ShowWarningMessage(string message)
    {
        throw new System.NotImplementedException();
    }

    public void ShowQuestionMessage(string message)
    {
        throw new System.NotImplementedException();
    }
}
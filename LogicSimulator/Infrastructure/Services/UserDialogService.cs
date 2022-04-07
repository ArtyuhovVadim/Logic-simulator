using System.Media;
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
    private readonly ErrorDialogWindowViewModel _errorDialogWindowViewModel;

    public UserDialogService(InfoDialogWindowViewModel infoDialogWindowViewModel,
                             ErrorDialogWindowViewModel errorDialogWindowViewModel)
    {
        _infoDialogWindowViewModel = infoDialogWindowViewModel;
        _errorDialogWindowViewModel = errorDialogWindowViewModel;
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

        SystemSounds.Beep.Play();
        infoDialogWindow.ShowDialog();
    }

    public void ShowErrorMessage(string message)
    {
        _errorDialogWindowViewModel.Message = message;

        var errorDialogWindow = new ErrorDialogWindow
        {
            DataContext = _errorDialogWindowViewModel,
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        _errorDialogWindowViewModel.Completed += () =>
        {
            errorDialogWindow.Close();
        };

        SystemSounds.Hand.Play();
        errorDialogWindow.ShowDialog();
    }

    public void ShowWarningMessage(string message)
    {
        SystemSounds.Beep.Play();
    }

    public void ShowQuestionMessage(string message)
    {
    }
}
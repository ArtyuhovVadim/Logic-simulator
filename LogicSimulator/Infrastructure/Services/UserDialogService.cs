using System.Media;
using System.Windows;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.UserDialogViewModels;
using LogicSimulator.Views.Windows.UserDialogWindows;

namespace LogicSimulator.Infrastructure.Services;

public class UserDialogService : IUserDialogService
{
    private readonly InfoDialogWindowViewModel _infoDialogWindowViewModel;
    private readonly ErrorDialogWindowViewModel _errorDialogWindowViewModel;
    private readonly WarningDialogWindowViewModel _warningDialogWindowViewModel;
    private readonly QuestionDialogWindowViewModel _questionDialogWindowViewModel;

    public UserDialogService(InfoDialogWindowViewModel infoDialogWindowViewModel,
                             ErrorDialogWindowViewModel errorDialogWindowViewModel,
                             WarningDialogWindowViewModel warningDialogWindowViewModel,
                             QuestionDialogWindowViewModel questionDialogWindowViewModel)
    {
        _infoDialogWindowViewModel = infoDialogWindowViewModel;
        _errorDialogWindowViewModel = errorDialogWindowViewModel;
        _warningDialogWindowViewModel = warningDialogWindowViewModel;
        _questionDialogWindowViewModel = questionDialogWindowViewModel;
    }

    public UserDialogResult ShowInfoMessage(string title, string message)
    {
        var result = UserDialogResult.None;
        
        _infoDialogWindowViewModel.Message = message;
        _infoDialogWindowViewModel.Title = title;

        var infoDialogWindow = new InfoDialogWindow
        {
            DataContext = _infoDialogWindowViewModel,
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        _infoDialogWindowViewModel.Completed += e =>
        {
            result = e;
            infoDialogWindow.Close();
        };

        SystemSounds.Beep.Play();
        infoDialogWindow.ShowDialog();

        return result;
    }

    public UserDialogResult ShowErrorMessage(string title, string message)
    {
        var result = UserDialogResult.None;

        _errorDialogWindowViewModel.Message = message;
        _errorDialogWindowViewModel.Title = title;

        var errorDialogWindow = new ErrorDialogWindow
        {
            DataContext = _errorDialogWindowViewModel,
            //TODO: ошибка при еще не открытом MainWindow
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        _errorDialogWindowViewModel.Completed += e =>
        {
            result = e;
            errorDialogWindow.Close();
        };

        SystemSounds.Hand.Play();
        errorDialogWindow.ShowDialog();

        return result;
    }

    public UserDialogResult ShowWarningMessage(string title, string message)
    {
        var result = UserDialogResult.None;

        _warningDialogWindowViewModel.Message = message;
        _warningDialogWindowViewModel.Title = title;

        var warningDialogWindow = new WarningDialogWindow
        {
            DataContext = _warningDialogWindowViewModel,
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        _warningDialogWindowViewModel.Completed += e =>
        {
            result = e;
            warningDialogWindow.Close();
        };

        SystemSounds.Beep.Play();
        warningDialogWindow.ShowDialog();

        return result;
    }

    public UserDialogResult ShowQuestionMessage(string title, string message)
    {
        var result = UserDialogResult.None;

        _questionDialogWindowViewModel.Message = message;
        _questionDialogWindowViewModel.Title = title;

        var questionDialogWindow = new QuestionDialogWindow()
        {
            DataContext = _questionDialogWindowViewModel,
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        _questionDialogWindowViewModel.Completed += e =>
        {
            result = e;
            questionDialogWindow.Close();
        };

        SystemSounds.Beep.Play();
        questionDialogWindow.ShowDialog();

        return result;
    }
}
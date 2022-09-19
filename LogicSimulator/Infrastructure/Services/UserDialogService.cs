using System.Media;
using System.Windows;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.UserDialogViewModels;
using LogicSimulator.ViewModels.UserDialogViewModels.Base;
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

    public UserDialogResult ShowInfoMessage(string title, string message) =>
        ShowUserDialogWindow<InfoDialogWindow>(title, message, _infoDialogWindowViewModel, SystemSounds.Beep);

    public UserDialogResult ShowErrorMessage(string title, string message) =>
        ShowUserDialogWindow<ErrorDialogWindow>(title, message, _errorDialogWindowViewModel, SystemSounds.Hand);

    public UserDialogResult ShowWarningMessage(string title, string message) =>
        ShowUserDialogWindow<WarningDialogWindow>(title, message, _warningDialogWindowViewModel, SystemSounds.Beep);

    public UserDialogResult ShowQuestionMessage(string title, string message) =>
        ShowUserDialogWindow<QuestionDialogWindow>(title, message, _questionDialogWindowViewModel, SystemSounds.Beep);

    private static UserDialogResult ShowUserDialogWindow<TWindow>(string title, string message, BaseUserDialogViewModel userDialogViewModel, SystemSound sound) 
        where TWindow : Controls.Window, new()
    {
        var result = UserDialogResult.None;

        userDialogViewModel.Message = message;
        userDialogViewModel.Title = title;

        var window = new TWindow
        {
            DataContext = userDialogViewModel,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        if (App.CurrentWindow is not null && App.CurrentWindow.IsLoaded)
        {
            window.Owner = App.CurrentWindow;
        }

        userDialogViewModel.Completed += e =>
        {
            result = e;
            window.Close();
        };

        sound.Play();
        window.ShowDialog();

        return result;
    }
}
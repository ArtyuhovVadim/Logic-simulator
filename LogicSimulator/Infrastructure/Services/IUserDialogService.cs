namespace LogicSimulator.Infrastructure.Services;

public interface IUserDialogService
{
    UserDialogResult ShowInfoMessage(string title, string message);

    UserDialogResult ShowErrorMessage(string title, string message);

    UserDialogResult ShowWarningMessage(string title, string message);

    UserDialogResult ShowQuestionMessage(string title, string message);
}
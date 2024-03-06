namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IUserDialogService
{
    UserDialogResult ShowInfoMessage(string title, string message);

    UserDialogResult ShowErrorMessage(string title, string message);

    UserDialogResult ShowWarningMessage(string title, string message);

    UserDialogResult ShowQuestionMessage(string title, string message);

    UserDialogResult OpenFileDialog(string title, IEnumerable<(string name, string pattern)> filters, out string path);

    UserDialogResult OpenFolderDialog(string title, out string path);
}
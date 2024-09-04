using LogicSimulator.Models.Common;
using LogicSimulator.ViewModels.Dialog.Base;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IUserDialogService
{
    T ShowDialog<T>() where T : BaseDialogViewModel;

    T ShowDialog<T>(Action<T> configure) where T : BaseDialogViewModel;

    UserDialogResult OpenFileDialog(string title, IEnumerable<(string name, string pattern)> filters, out string path);

    UserDialogResult OpenFolderDialog(string title, out string path);
}
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.Dialog;

namespace LogicSimulator.Infrastructure.ExtensionMethods;

public static class UserDialogExtensionMethods
{
    public static void ShowInfoMessage(this IUserDialogService service, string title, string message) =>
        service.ShowDialog<InfoDialogViewModel>(vm =>
        {
            vm.Title = title;
            vm.Message = message;
            vm.IconSource = new Uri("pack://application:,,,/Resources/Icons/info-icon512x512.png");
        });

    public static void ShowErrorMessage(this IUserDialogService service, string title, string message) =>
        service.ShowDialog<ErrorDialogViewModel>(vm =>
        {
            vm.Title = title;
            vm.Message = message;
            vm.IconSource = new Uri("pack://application:,,,/Resources/Icons/error-icon512x515.png");
        });
}
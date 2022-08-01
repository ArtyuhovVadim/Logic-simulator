using LogicSimulator.ViewModels.UserDialogViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LogicSimulator.ViewModels.Infrastructure;

public class ViewModelLocator
{
    public static MainWindowViewModel MainWindowViewModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();

    public static SchemeSceneViewModel SchemeSceneViewModel => App.Host.Services.GetRequiredService<SchemeSceneViewModel>();

    public static InfoDialogWindowViewModel InfoDialogWindowViewModel => App.Host.Services.GetRequiredService<InfoDialogWindowViewModel>();

    public static ErrorDialogWindowViewModel ErrorDialogWindowViewModel => App.Host.Services.GetRequiredService<ErrorDialogWindowViewModel>();

    public static WarningDialogWindowViewModel WarningDialogWindowViewModel => App.Host.Services.GetRequiredService<WarningDialogWindowViewModel>();

    public static QuestionDialogWindowViewModel QuestionDialogWindowViewModel => App.Host.Services.GetRequiredService<QuestionDialogWindowViewModel>();
}
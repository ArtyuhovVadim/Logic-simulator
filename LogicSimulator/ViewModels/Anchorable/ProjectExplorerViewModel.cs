using LogicSimulator.Infrastructure.Messages;
using LogicSimulator.ViewModels.Anchorable.Base;
using WpfExtensions.Mvvm.Commands;
using WpfExtensions.Mvvm.Messaging;

namespace LogicSimulator.ViewModels.Anchorable;

public class ProjectExplorerViewModel : ToolViewModel, IRecipient<ProjectLoadedMessage>
{
    private readonly IMessageBus _messageBus;

    public ProjectExplorerViewModel(IMessageBus messageBus)
    {
        _messageBus = messageBus;
        _messageBus.RegisterHandler(this);
    }

    public override string Title => "Обозреватель проекта";

    #region ProjectViewModel

    private ProjectViewModel? _projectViewModel;

    public ProjectViewModel? ProjectViewModel
    {
        get => _projectViewModel;
        private set => Set(ref _projectViewModel, value);
    }

    #endregion

    #region OpenSchemeCommand

    private ICommand? _openSchemeCommand;

    public ICommand OpenSchemeCommand => _openSchemeCommand ??= new LambdaCommand<object>(p =>
    {
        if (p is not SchemeViewModel schemeViewModel) return;
        _messageBus.Send(new DocumentOpenedMessage(schemeViewModel));
    });

    #endregion

    public void Receive(ProjectLoadedMessage message) => ProjectViewModel = message.Project;
}
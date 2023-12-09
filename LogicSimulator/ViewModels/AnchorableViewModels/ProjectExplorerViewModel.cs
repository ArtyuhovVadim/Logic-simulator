using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

public class ProjectExplorerViewModel : ToolViewModel
{
    public event Action<SchemeViewModel> SchemeOpened;

    public override string Title => "Обозреватель проекта";

    #region ProjectViewModel

    private ProjectViewModel _projectViewModel;

    public ProjectViewModel ProjectViewModel
    {
        get => _projectViewModel;
        set
        {
            if (Set(ref _projectViewModel, value))
            {
                OnPropertyChanged(nameof(ProjectViewModels));
            }
        }
    }

    #endregion

    public IEnumerable<ProjectViewModel> ProjectViewModels => new[] { ProjectViewModel };

    #region OpenSchemeCommand

    private ICommand _openSchemeCommand;

    public ICommand OpenSchemeCommand => _openSchemeCommand ??= new LambdaCommand(p =>
    {
        if (p is not SchemeViewModel schemeViewModel) return;

        SchemeOpened?.Invoke(schemeViewModel);
    });

    #endregion
}
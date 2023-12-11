using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class ProjectViewModel : BindableBase
{
    private readonly ISchemeViewModelFactory _schemeFactory;

    public Project Project { get; }

    public ProjectViewModel(Project project, ISchemeViewModelFactory schemeFactory)
    {
        _schemeFactory = schemeFactory;
        Project = project;

        foreach (var scheme in project.Schemes)
        {
            _schemes.Add(_schemeFactory.Create(scheme));
        }
    }

    #region Version

    public string Version
    {
        get => Project.Version;
        set
        {
            Project.Version = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Name

    public string Name
    {
        get => Project.Name;
        set
        {
            //TODO: Реализовать переименование файла проекта
            Project.Name = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Schemes

    private ObservableCollection<SchemeViewModel> _schemes = [];

    public ObservableCollection<SchemeViewModel> Schemes
    {
        get => _schemes;
        private set => Set(ref _schemes, value);
    }

    #endregion
}
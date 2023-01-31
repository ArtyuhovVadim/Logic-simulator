using System.Collections.ObjectModel;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class ProjectViewModel : BindableBase
{
    public Project Project { get; }

    public ProjectViewModel(Project project)
    {
        Project = project;

        _schemes = new ObservableCollection<SchemeViewModel>();

        foreach (var scheme in project.Schemes)
        {
            _schemes.Add(new SchemeViewModel(scheme));
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

    private ObservableCollection<SchemeViewModel> _schemes;

    public ObservableCollection<SchemeViewModel> Schemes
    {
        get => _schemes;
        private set => Set(ref _schemes, value);
    }

    #endregion
}
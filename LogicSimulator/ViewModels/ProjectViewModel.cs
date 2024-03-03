using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class ProjectViewModel : BindableBase, IModelBased<Project>
{
    public ProjectViewModel(Project project, ISchemeViewModelFactory schemeFactory)
    {
        Model = project;
        _schemes = new ObservableCollectionEx<SchemeViewModel, Scheme>(Model.Schemes, schemeFactory.Create);
    }

    #region Model

    public Project Model { get; }

    #endregion

    #region Version

    public string Version
    {
        get => Model.Version;
        set
        {
            Model.Version = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Name

    public string Name
    {
        get => Model.Name;
        set
        {
            //TODO: Реализовать переименование файла проекта
            Model.Name = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Schemes

    private readonly ObservableCollectionEx<SchemeViewModel, Scheme> _schemes;

    public ObservableCollection<SchemeViewModel> Schemes => _schemes;

    #endregion
}